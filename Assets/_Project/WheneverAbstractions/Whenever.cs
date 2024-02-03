using System;

[Flags]
public enum Target
{
    Player = 1>>0,
    Enemy = 1>>1,
    Any = 0xFFFF
}

public class Whenever
{
    private WheneverFilter filter;
    public delegate void Effect(DamagePackage damagePackage, Combatant triggerTarget);
    public Effect effect;

    public void SetTrigger(DamageType trigger)
    {
        this.filter.trigger = trigger;
    }
    
    public void SetTarget(Target target)
    {
        this.filter.target = target;
    }

    public void TryTrigger(DamagePackage damagePackage, Combatant triggerTarget)
    {
        if (triggerTarget != damagePackage.target) return;
        
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

public class WheneverFilter
{
    public DamageType trigger;
    public Target target;

    public bool CanTrigger(DamagePackage damagePackage, DamageContext context, Combatant triggerTarget)
    {
        if (triggerTarget != context.target) return false;
        
        var targetEnumType = triggerTarget.combatantType switch
        {
            CombatantType.Player => Target.Player,
            CombatantType.Enemy => Target.Enemy,
            _ => throw new ArgumentOutOfRangeException()
        };
        if((target & targetEnumType) == 0) return false;
        if (damagePackage.damageType != trigger) return false;
        return true;
    }
}