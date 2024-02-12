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
            var effectTypeToDeserialize = allEffectTypesFromLoadedAssemblies
                .FirstOrDefault(x => x.typeKey == typeIndicator.type)?.type;
            if(effectTypeToDeserialize == null)
            {
                return (null, $"Could not find effect type {typeIndicator.type}");
            }
            
            var effect = (IEffect<TInspectWorld, TCommandWorld>)JsonUtility.FromJson(json, effectTypeToDeserialize);
            return (effect, null);
        }
    }
}