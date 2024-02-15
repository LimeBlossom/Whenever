using System;
using System.Linq;
using Serialization;
using UnityEngine;

public record CompositeWheneverFilter<TInspectWorld, TCommandWorld> : IWheneverFilter<TInspectWorld, TCommandWorld>
    where TInspectWorld : IInspectWorld
    where TCommandWorld : ICommandWorld
{
    private readonly Func<IDescriptionContext, string> overrideDescription = null;
    internal IWheneverFilter<TInspectWorld, TCommandWorld>[] filters;
    
    public CompositeWheneverFilter(params IWheneverFilter<TInspectWorld, TCommandWorld>[] filters)
    {
        this.filters = filters;
    }
    
    public CompositeWheneverFilter(Func<IDescriptionContext, string> overrideDescription, params IWheneverFilter<TInspectWorld, TCommandWorld>[] filters)
    {
        this.overrideDescription = overrideDescription;
        this.filters = filters;
    }

    public bool TriggersOn(InitiatedCommand<TCommandWorld> initiatedCommand, TInspectWorld world)
    {
        return filters.All(filter => filter.TriggersOn(initiatedCommand, world));
    }

    public string Describe(IDescriptionContext context)
    {
        if (overrideDescription != null) return overrideDescription(context);
        
        return string.Join(" and ", filters?.Select(filter => filter.Describe(context)) ?? Array.Empty<string>());
    }
}