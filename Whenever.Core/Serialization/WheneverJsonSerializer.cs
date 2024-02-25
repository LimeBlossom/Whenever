using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

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

        public (IWheneverFilter<TInspectWorld, TCommandWorld>? res, string? error) DeserializeFilter(string json)
        {
            return PolymorphicDeserialize<IWheneverFilter<TInspectWorld, TCommandWorld>>(json);
        }
        
        private class PartialTypeIndicator
        {
            public string type;
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
            try
            {
                var typeIndicator = JsonUtility.FromJson<PartialTypeIndicator>(json);
                var typeToDeserialize = allEffectTypesFromLoadedAssemblies
                    .FirstOrDefault(x => x.typeKey == typeIndicator.type)?.type;
                if(typeToDeserialize == null)
                {
                    return (null, $"Could not find type {typeIndicator.type}");
                }
                
                var deserialized = (T)JsonUtility.FromJson(json, typeToDeserialize);
                return (deserialized, null);
            }
            catch (System.ArgumentException e)
            {
                return (null, e.Message);
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
}