using System;
using _Project.WheneverAbstractions;

[Flags]
public enum Target
{
    Player = 1>>0,
    Enemy = 1>>1,
    Any = 0xFFFF
}

public class Whenever
{
    private WheneverDamageFilter damageFilter;
    private IEffect effect;

    public Whenever(WheneverDamageFilter damageFilter, IEffect effect)
    {
        this.damageFilter = damageFilter;
        this.effect = effect;
    }

    public void TryTrigger(DamagePackage damagePackage, Combatant triggerTarget)
    {
        throw new NotImplementedException();
        // if (triggerTarget != damagePackage.target) return;
        //
        // var targetEnumType = triggerTarget.combatantType switch
        // {
        //     CombatantType.Player => Target.Player,
        //     CombatantType.Enemy => Target.Enemy,
        //     _ => throw new ArgumentOutOfRangeException()
        // };
        // if((target & targetEnumType) == 0) return;
        // if (damagePackage.damageType != trigger) return;
        //
        //     /*newDamagePackage =*/ effect?.Invoke(damagePackage, triggerTarget);
        // if success then WheneverManager.CheckWhenevers
    }
}

public interface IWheneverFilter
{
    public bool TriggersOn(IWorldCommand command, ICommandInitiator initiator, GlobalCombatWorld world);
}

public class WheneverDamageFilter : IWheneverFilter
{
    public DamageType trigger;
    public Target targetType;

    public bool CanTrigger(DamagePackage damagePackage, DamageContext context, Combatant triggerTarget)
    {
        if (triggerTarget != context.target) return false;
        
        var targetEnumType = triggerTarget.combatantType switch
        {
            CombatantType.Player => Target.Player,
            CombatantType.Enemy => Target.Enemy,
            _ => throw new ArgumentOutOfRangeException()
        };
        if((targetType & targetEnumType) == 0) return false;
        if (damagePackage.damageType != trigger) return false;
        return true;
    }

    public bool TriggersOn(IWorldCommand command, ICommandInitiator initiator, GlobalCombatWorld world)
    {
        if(command is DamageCommand damageCommand)
        {
            if(initiator is CombatantCommandInitiator combatantCommandInitiator)
            {
                var combatantData = world.GetCombatantData(damageCommand.Target);
                return CanTrigger(damageCommand.damagePackage, combatantData., combatantCommandInitiator.Initiator);
            }
        }
        throw new NotImplementedException();
    }
}