using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using JetBrains.Annotations;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Serialization
{
    public class WheneverJsonSerializer<TInspectWorld, TCommandWorld>
        where TInspectWorld : IInspectWorld
        where TCommandWorld : ICommandWorld
    {
        
        public (IEffect<TInspectWorld, TCommandWorld>? res, string? error) DeserializeEffect(string json)
        {
            return PolymorphicDeserialize<IEffect<TInspectWorld, TCommandWorld>>(json);
        }
        
        public (string? res, string? error) Serialize(Whenever<TInspectWorld, TCommandWorld> effect)
        {
            return (null, "not implemented");
        }
        
        public (Whenever<TInspectWorld, TCommandWorld>? res, string? error) DeserializeWhenever(string json)
        {
            return (null, "not implemented");
        }
        
        public (string? rest, string? error) SerializeEffect(IEffect<TInspectWorld, TCommandWorld> effect)
        {
            return PolymorphicSerialize(effect);
        }

        public (IWheneverFilter<TInspectWorld, TCommandWorld>? res, string? error) DeserializeFilter(string json)
        {
            return PolymorphicDeserialize<IWheneverFilter<TInspectWorld, TCommandWorld>>(json);
        }
        
        public (string? rest, string? error) SerializeFilter(IWheneverFilter<TInspectWorld, TCommandWorld> filter)
        {
            return PolymorphicSerialize(filter);
        }
        
        
        private (string? res, string? error) PolymorphicSerialize<T>(T obj)
        {
            var objType = obj.GetType();
            var typeKey = objType.GetCustomAttribute<PolymorphicSerializableAttribute>()?.typeKey;
            if(typeKey == null)
            {
                return (null, "Type does not have PolymorphicSerializableAttribute");
            }
            
            var unitySerializedJson = JsonUtility.ToJson(obj);
            
            var expectedStart = @"{""";
            if (!unitySerializedJson.StartsWith(expectedStart))
            {
                return (null, $"Expected start of json to be {expectedStart}");
            }
            
            var replacedStart = @"{""type"":""" + typeKey + @""",""";
            var unitySerializedJsonWithType = replacedStart + unitySerializedJson.Substring(expectedStart.Length);
            return (unitySerializedJsonWithType, null);
        }
        
        public (T? res, string? error) PolymorphicDeserialize<T>(string json) where T : class
        {
            var filterType = typeof(T);
            var allEffectTypesFromLoadedAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => x.GetCustomAttribute<PolymorphicSerializableAttribute>() != null)
                .Select(x => GetBaseTypeOrGenericisedInstance(x, filterType))
                .Where(x => x != null)
                .Select(type => new
                {
                    type, 
                    type.GetCustomAttribute<PolymorphicSerializableAttribute>()?.typeKey
                })
                .Where(x => x.typeKey != null)
                .ToArray();
            
            var (typeIndicator, error) = Try<PartialTypeIndicator, ArgumentException>(() => JsonUtility.FromJson<PartialTypeIndicator>(json));
            if (error != null)
            {
                return (null, error.Message);
            }
            var typeToDeserialize = allEffectTypesFromLoadedAssemblies
                .FirstOrDefault(x => x.typeKey == typeIndicator.type)?.type;
            if(typeToDeserialize == null)
            {
                return (null, $"Could not find type {typeIndicator.type}");
            }
            
            var (deserialized, deserializationError) = Try<object, ArgumentException>(() => JsonUtility.FromJson(json, typeToDeserialize));
            if(deserialized == null)
            {
                return (null, deserializationError.Message);
            }
            if (deserialized is not T deserializedAsT)
            {
                return (null, $"Deserialized object is not {filterType.Name}");
            }
            return (deserializedAsT, null);
        }
        
        private (TSuccess res, TException error) Try<TSuccess, TException>(Func<TSuccess> fn)
            where TException : Exception
        {
            try
            {
                return (fn(), null);
            }
            catch (TException e)
            {
                return (default, e);
            }
        }
        
        private Type GetBaseTypeOrGenericisedInstance(Type assemblyType, Type targetType)
        {
            if(targetType.IsAssignableFrom(assemblyType) && !assemblyType.IsAbstract)
            {
                return assemblyType;
            }
            if (assemblyType.IsGenericType)
            {
                // construct a version of the generic type with correct type parameters,
                // assuming they are identical to the target type's type parameters 
                var targetGenericParameters = targetType.GetGenericArguments();
                var assemblyGenericParameters = assemblyType.GetGenericArguments();
                if (targetGenericParameters.Length != assemblyGenericParameters.Length)
                {
                    throw new Exception("Generic type parameters do not match");
                }
                return assemblyType.MakeGenericType(targetGenericParameters);
            }

            return null;
        }
    }
    
    [Serializable]
    internal class PartialTypeIndicator
    {
        [SerializeField]
        public string type;
    }


    internal interface IPartialTypeExtractor<out T>
    {
        public T Data { get; }
    }

    internal class PartialTypeContainer : PartialTypeIndicator
    {
        public object data;
    }
    
    [Serializable]
    internal class PartialTypeContainer<T> : PartialTypeIndicator, IPartialTypeExtractor<T>
    {
        [SerializeField]
        public T data;
        public T Data => data;
    }

    public interface IObjectGraphSerializer
    {
        public string Typename { get; }
    }

    internal class Idk : CustomCreationConverter<IPolymorphicSerializable>
    {
        public override IPolymorphicSerializable Create(Type objectType)
        {
            throw new NotImplementedException();
        }
    }
    
    internal class TypenameConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if(value == null)
            {
                writer.WriteNull();
                return;
            }
            
            var objectType = value.GetType();
            var typeKey = objectType.GetCustomAttribute<PolymorphicSerializableAttribute>()?.typeKey;
            if(typeKey == null)
            {
                throw new Exception("Type does not have PolymorphicSerializableAttribute");
            }
            
            JToken t;
            serializer.Converters.Remove(this);
            var converters = serializer.Converters.ToArray();
            try
            {
                 t = JToken.FromObject(value, serializer);
            }
            finally
            {
                serializer.Converters.Add(this);
            }

            if (t.Type != JTokenType.Object)
            {
                throw new Exception("Written json must be a json object");
            }

            JObject o = (JObject)t;
                
            o.AddFirst(new JProperty("type", typeKey));

            o.WriteTo(writer, converters);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jobject = JObject.Load(reader);
            var typename = (string)JObject.Load(reader)["type"];
        }

        public override bool CanConvert(Type objectType)
        {
            var attr = objectType.GetCustomAttribute<PolymorphicSerializableAttribute>();
            return attr != null;
        }
    }

}