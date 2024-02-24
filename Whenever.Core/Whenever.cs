using System;
using System.Collections.Generic;
using System.Linq;
using CoreFac;

public record Whenever<TInspectWorld, TCommandWorld> : IDescribableWithContext
    where TInspectWorld : IInspectWorld
    where TCommandWorld : ICommandWorld
{
    public readonly IWheneverFilter<TInspectWorld, TCommandWorld> filter;
    public readonly IEffect<TInspectWorld, TCommandWorld> effect;
    
    /// <summary>
    /// a unique id. will be unique for each unique whenever contained inside a wheneverManager.
    /// </summary>
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Whenever(IWheneverFilter<TInspectWorld, TCommandWorld> filter, IEffect<TInspectWorld, TCommandWorld> effect)
    {
        this.filter = filter;
        this.effect = effect;
    }

    public IEnumerable<InitiatedCommand<TCommandWorld>> GetTriggeredCommands(
        InitiatedCommand<TCommandWorld> command,
        TInspectWorld world)
    {
        if (!filter.TriggersOn(command, world)) return Enumerable.Empty<InitiatedCommand<TCommandWorld>>();

        var nextInitiator = Initiators.FromEffectOf(command.initiator);
            
        return effect
            .ApplyEffect(command, world)
            .Select(x => new InitiatedCommand<TCommandWorld>(x, nextInitiator));
    }
    internal Whenever<TInspectWorld, TCommandWorld> ForceRegenerateIdentifier()
    {
        return this with
        {
            Id = Guid.NewGuid()
        };
    }
    public string Describe(IDescriptionContext context)
    {
        return $"When {filter.Describe(context)}; {effect.Describe(context)}";
    }
}