using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum Target
{
    Player = 1>>0,
    Enemy = 1>>1,
    Any = 0xFFFF
}

public class Whenever
{
    private DamageType trigger;
    private bool onEnemy;
    private Target target;
    private int maxDistance;

    public delegate void Effect(DamagePackage damagePackage, Combatant triggerTarget);
    public Effect effect;

    public void SetTrigger(DamageType trigger)
    {
        this.trigger = trigger;
    }
    
    public void SetTarget(Target target)
    {
        this.target = target;
    }

    public void TryTrigger(DamagePackage damagePackage, Combatant triggerTarget)
    {
        var targetEnumType = triggerTarget.combatantType switch
        {
            CombatantType.Player => Target.Player,
            CombatantType.Enemy => Target.Enemy,
            _ => throw new ArgumentOutOfRangeException()
        };
        if((target & targetEnumType) == 0) return;
        if (damagePackage.damageType != trigger) return;
        
            /*newDamagePackage =*/ effect?.Invoke(damagePackage, triggerTarget);
        // if success then WheneverManager.CheckWhenevers
    }
}
