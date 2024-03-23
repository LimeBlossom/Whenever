using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public abstract record EffectAliasedTargetEffect<TInspectWorld, TCommandWorld> : IEffect<TInspectWorld, TCommandWorld>
    where TInspectWorld : IInspectWorld
    where TCommandWorld : ICommandWorld
{
    [SerializeField]
    private CombatantAlias combatant;

    public CombatantAlias CombatantTarget => combatant;
    public EffectAliasedTargetEffect(CombatantAlias combatant)
    {
        this.combatant = combatant;
    }
    
    public IEnumerable<IWorldCommand<TCommandWorld>> ApplyEffect(
        InitiatedCommand<TCommandWorld> command,
        IAliasCombatantIds aliaser,
        TInspectWorld world)
    {
        var target = aliaser.GetIdForAlias(combatant);
        if (target == null)
        {
            Debug.LogWarning($"#{this.GetType().Name}: Could not find target for alias '{combatant}'");
            return Enumerable.Empty<IWorldCommand<TCommandWorld>>();
        }

        return this.ApplyEffectToTarget(target, command, world);
    }
    public string Describe(IDescriptionContext context)
    {
        return DescribeOnTarget() + context.ToAliasAsDirectSubject(combatant);
    }
        
    protected abstract string DescribeOnTarget();
        
    protected abstract IEnumerable<IWorldCommand<TCommandWorld>> ApplyEffectToTarget(
        CombatantId target,
        InitiatedCommand<TCommandWorld> command,
        TInspectWorld world);
}