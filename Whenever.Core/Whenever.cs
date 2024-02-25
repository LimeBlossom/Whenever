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

    private readonly IAliasCombatantIds aliaser;
    
    /// <summary>
    /// a unique id. will be unique for each unique whenever contained inside a wheneverManager.
    /// </summary>
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Whenever(
        IWheneverFilter<TInspectWorld, TCommandWorld> filter,
        IEffect<TInspectWorld, TCommandWorld> effect,
        IAliasCombatantIds bakedAliases = null)
    {
        this.filter = filter;
        this.effect = effect;
        this.aliaser = bakedAliases;
    }
    
    public Whenever<TInspectWorld, TCommandWorld> BakeCombatantAlias(IAliasCombatantIds aliases)
    {
        return new Whenever<TInspectWorld, TCommandWorld>(filter, effect, aliases);
    }

    public IEnumerable<InitiatedCommand<TCommandWorld>> GetTriggeredCommands(
        InitiatedCommand<TCommandWorld> command,
        IAliasCombatantIds contextAliaser,
        TInspectWorld world)
    {
        var aliasContext = contextAliaser
            .OverrideWith(aliaser)
            .OverrideWithCommandContext(command);
        if (!filter.TriggersOn(command, aliasContext, world)) return Enumerable.Empty<InitiatedCommand<TCommandWorld>>();

        var nextInitiator = Initiators.FromEffectOf(command.initiator);
            
        return effect
            .ApplyEffect(command, aliasContext, world)
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