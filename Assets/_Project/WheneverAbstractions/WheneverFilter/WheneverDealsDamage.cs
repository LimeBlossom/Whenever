using System;
using WheneverAbstractions._Project.WheneverAbstractions.CommandInitiators;
using WheneverAbstractions._Project.WheneverAbstractions.Commands;

namespace WheneverAbstractions._Project.WheneverAbstractions.WheneverFilter
{
    
    /// <summary>
    /// Only apply when the command initiator is of a specific combatant type, and when dealing a specific type of damage to some other target.
    /// </summary>
    public record WheneverDealsDamage : IWheneverFilter
    {
        public DamageType validDamageType;
        public WheneverCombatantTypeFilter wheneverCombatantTypeFilterType;

        private bool CanTrigger(DamagePackage damagePackage, CombatantType combatantType)
        {
            if(damagePackage.damageType != validDamageType) return false;
        
            var targetEnumType = combatantType switch
            {
                CombatantType.Player => WheneverCombatantTypeFilter.Player,
                CombatantType.Enemy => WheneverCombatantTypeFilter.Enemy,
                _ => throw new ArgumentOutOfRangeException()
            };
            if((wheneverCombatantTypeFilterType & targetEnumType) == 0) return false;
            return true;
        }
        
        public bool TriggersOn(InitiatedCommand initiatedCommand, IInspectableWorld world)
        {
            if(initiatedCommand.command is DamageCommand damageCommand)
            {
                if(initiatedCommand.initiator.TryAsOrRecursedFrom<CombatantCommandInitiator>(out var initiator))
                {
                    var combatantType = world.CombatantData(initiator.Initiator).GetCombatantType();
                    return CanTrigger(damageCommand.damagePackage, combatantType);
                }
            }

            return false;
        }
    }
}