﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract record EffectSpecificTargetEffect<TInspectWorld, TCommandWorld> : IEffect<TInspectWorld, TCommandWorld>
    where TInspectWorld : IInspectWorld
    where TCommandWorld : ICommandWorld
{
    public readonly CombatantId specificTarget;

    public EffectSpecificTargetEffect(CombatantId specificTarget)
    {
        this.specificTarget = specificTarget;
    }
    
    public IEnumerable<IWorldCommand<TCommandWorld>> ApplyEffect(InitiatedCommand<TCommandWorld> command, TInspectWorld world)
    {
        if(!world.Contains(specificTarget)) return Enumerable.Empty<IWorldCommand<TCommandWorld>>();
        return ApplyEffectTo(specificTarget, world);
    }

    public string Describe(IDescriptionContext context)
    {
        return DescribeEffect() + $" to {context.NameOf(specificTarget)}";
    }
        
    public abstract string DescribeEffect();

    protected abstract IEnumerable<IWorldCommand<TCommandWorld>> ApplyEffectTo(CombatantId target, TInspectWorld world);
}