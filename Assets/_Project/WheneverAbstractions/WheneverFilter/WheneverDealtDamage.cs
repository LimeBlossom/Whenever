using System;
using WheneverAbstractions._Project.WheneverAbstractions.Commands;

namespace WheneverAbstractions._Project.WheneverAbstractions.WheneverFilter
{
    public record WheneverDealtDamage : IWheneverFilter
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
                var targetData = world.CombatantData(damageCommand.Target).GetCombatantType();
                return CanTrigger(damageCommand.damagePackage, targetData);
            }

            return false;
        }
    }
}