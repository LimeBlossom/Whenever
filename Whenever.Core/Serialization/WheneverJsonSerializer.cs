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
        private class PartialTypeIndicator
        {
            public string type;
        }
        
        public (IEffect<TInspectWorld, TCommandWorld>? res, string? error) DeserializeEffect(string json)
        {
            try
            {
                var effectType = typeof(IEffect<TInspectWorld, TCommandWorld>);
                
                var allEffectTypesFromLoadedAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .Where(x => effectType.IsAssignableFrom(x) && !x.IsAbstract)
                    .Select(type => new
                    {
                        type, 
                        type.GetCustomAttribute<PolymorphicSerializableAttribute>()?.typeKey
                    })
                    .Where(x => x.typeKey != null)
                    .ToArray();

                    var typeIndicator = JsonUtility.FromJson<PartialTypeIndicator>(json);
                var typeToDeserialize = allEffectTypesFromLoadedAssemblies
                    .FirstOrDefault(x => x.typeKey == typeIndicator.type)?.type;
                if(typeToDeserialize == null)
                {
                    return (null, $"Could not find type {typeIndicator.type}");
                }
                
                var effect = (IEffect<TInspectWorld, TCommandWorld>)JsonUtility.FromJson(json, typeToDeserialize);
                return (effect, null);
            }
            catch (System.ArgumentException e)
            {
                return (null, e.Message);
            }
        }

        public (IWheneverFilter<TInspectWorld, TCommandWorld>? res, string? error) DeserializeFilter(string json)
        {
            try
            {
                var filterType = typeof(IWheneverFilter<TInspectWorld, TCommandWorld>);
                
                var allEffectTypesFromLoadedAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .Where(x => filterType.IsAssignableFrom(x) && !x.IsAbstract)
                    .Select(type => new
                    {
                        type, 
                        type.GetCustomAttribute<PolymorphicSerializableAttribute>()?.typeKey
                    })
                    .Where(x => x.typeKey != null)
                    .ToArray();
                
                var typeIndicator = JsonUtility.FromJson<PartialTypeIndicator>(json);
                var typeToDeserialize = allEffectTypesFromLoadedAssemblies
                    .FirstOrDefault(x => x.typeKey == typeIndicator.type)?.type;
                if(typeToDeserialize == null)
                {
                    return (null, $"Could not find type {typeIndicator.type}");
                }
                
                var deserialized = (IWheneverFilter<TInspectWorld, TCommandWorld>)JsonUtility.FromJson(json, typeToDeserialize);
                return (deserialized, null);
            }
            catch (System.ArgumentException e)
            {
                return (null, e.Message);
            }
        }
    }
}