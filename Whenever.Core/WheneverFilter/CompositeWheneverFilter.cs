using System;
using System.Linq;
using Serialization;
using UnityEngine;

[PolymorphicSerializable("CompositeWhenever"), Serializable]
public record CompositeWheneverFilter<TInspectWorld, TCommandWorld> : IWheneverFilter<TInspectWorld, TCommandWorld>
    where TInspectWorld : IInspectWorld
    where TCommandWorld : ICommandWorld
{
    [SerializeField]
    private IWheneverFilter<TInspectWorld, TCommandWorld>[] filters;
    
    public CompositeWheneverFilter(params IWheneverFilter<TInspectWorld, TCommandWorld>[] filters)
    {
        this.filters = filters;
    }

    public bool TriggersOn(InitiatedCommand<TCommandWorld> initiatedCommand, TInspectWorld world)
    {
        return filters.All(filter => filter.TriggersOn(initiatedCommand, world));
    }

    public string Describe()
    {
        return string.Join(" and ", filters?.Select(filter => filter.Describe()) ?? Array.Empty<string>());
    }
}