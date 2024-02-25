﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract record EffectInitiatorEffect<TInspectWorld, TCommandWorld> : IEffect<TInspectWorld, TCommandWorld>
    where TInspectWorld : IInspectWorld
    where TCommandWorld : ICommandWorld
{
    public IEnumerable<IWorldCommand<TCommandWorld>> ApplyEffect(
        InitiatedCommand<TCommandWorld> command,
        IAliasCombatantIds aliaser,
        TInspectWorld world)
    {
        if(!command.initiator.TryAsOrRecursedFrom<CombatantCommandInitiator>(out var initiator))
        {
            Debug.LogWarning($"Initiator effect {GetType().Name} can only apply damage on a combatant command initiator");
            return Enumerable.Empty<IWorldCommand<TCommandWorld>>();
        }
        return this.ApplyEffectToInitiator(initiator.Initiator, world);
    }

    public string Describe(IDescriptionContext context)
    {
        return DescribeOnInitiator() + context.ToInitiatorAsDirectSubject();
    }
        
    public abstract string DescribeOnInitiator();

    protected abstract IEnumerable<IWorldCommand<TCommandWorld>> ApplyEffectToInitiator(CombatantId initiator, TInspectWorld world);
}