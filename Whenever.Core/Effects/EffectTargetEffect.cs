﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract record EffectTargetEffect<TInspectWorld, TCommandWorld> : IEffect<TInspectWorld, TCommandWorld>
    where TInspectWorld : IInspectWorld
    where TCommandWorld : ICommandWorld
{
    public IEnumerable<IWorldCommand<TCommandWorld>> ApplyEffect(
        InitiatedCommand<TCommandWorld> command,
        IAliasCombatantIds aliaser,
        TInspectWorld world)
    {
        if (command.command is not IGenericTargetedWorldCommand<TCommandWorld> targetedCommand)
        {
            Debug.LogWarning($"Target effect {GetType().Name} can only apply on commands that target at least one combatant");
            return Enumerable.Empty<IWorldCommand<TCommandWorld>>();
        }
        return this.ApplyEffectToTarget(targetedCommand.Target, world);
    }
    public string Describe(IDescriptionContext context)
    {
        return DescribeOnTarget() + context.ToTargetAsDirectSubject();
    }
        
    protected abstract string DescribeOnTarget();
        
    protected abstract IEnumerable<IWorldCommand<TCommandWorld>> ApplyEffectToTarget(CombatantId target, TInspectWorld world);
}