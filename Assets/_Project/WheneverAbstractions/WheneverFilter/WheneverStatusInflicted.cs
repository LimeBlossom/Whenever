using System;
using System.Linq;
using WheneverAbstractions._Project.WheneverAbstractions.CommandInitiators;
using WheneverAbstractions._Project.WheneverAbstractions.Commands;
using WheneverAbstractions._Project.WheneverAbstractions.StatusEffects;

namespace WheneverAbstractions._Project.WheneverAbstractions.WheneverFilter
{
    public record WheneverStatusInflicted : IWheneverFilter
    {
        public DamageType dotDamageType;
        public WheneverCombatantTypeFilter wheneverCombatantTypeFilterType;

        private bool CanTrigger(StatusEffect statusEffect, CombatantType combatantType)
        {
            if (statusEffect is not DotStatus dotStatus) return false;
            if (dotStatus.damagePackage.damageType != dotDamageType) return false;
        
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
            if(initiatedCommand.command is AddStatusEffectCommand statusCommand)
            {
                var targetData = world.CombatantData(statusCommand.Target).GetCombatantType();
                return CanTrigger(statusCommand.statusEffect, targetData);
            }

            return false;
        }
    }
}