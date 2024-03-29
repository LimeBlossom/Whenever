﻿using System;
using System.Collections.Generic;
using System.Linq;

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

    public bool TriggersOn(
        InitiatedCommand<TCommandWorld> initiatedCommand,
        IAliasCombatantIds aliaser,
        TInspectWorld world)
    {
        return filters.All(filter => filter.TriggersOn(initiatedCommand, aliaser, world));
    }

    public string Describe(IDescriptionContext context)
    {
        if (overrideDescription != null) return overrideDescription(context);
        
        return string.Join(" and ", filters?.Select(filter => filter.Describe(context)) ?? Array.Empty<string>());
    }
}

public static class OptionallyCompositeWheneverFilters{
    
    public static IEnumerable<IWheneverFilter<TInspect, TCommand>> Flatten<TInspect, TCommand>(this IWheneverFilter<TInspect, TCommand> filter)
        where TInspect : IInspectWorld
        where TCommand : ICommandWorld
    {
        if (filter is CompositeWheneverFilter<TInspect, TCommand> composite)
        {
            return composite.filters.SelectMany(Flatten);
        }
        return new[] {filter};
    }
    
}