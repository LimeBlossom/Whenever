using System;
using System.Linq;
using WheneverAbstractions._Project.WheneverAbstractions.CommandInitiators;
using WheneverAbstractions._Project.WheneverAbstractions.Commands;

namespace WheneverAbstractions._Project.WheneverAbstractions.WheneverFilter
{
    public class WheneverDealtDamage : IWheneverFilter
    {
        public DamageType validDamageType;
        public WheneverCombatantTypeFilter wheneverCombatantTypeFilterType;

        private bool CanTrigger(DamagePackage damagePackage, ICombatantData triggerTarget)
        {
            if(damagePackage.damageType != validDamageType) return false;
        
            var targetEnumType = triggerTarget.GetCombatantType() switch
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
                var targetData = world.CombatantData(damageCommand.Target);
                return CanTrigger(damageCommand.damagePackage, targetData);
            }

            return false;
        }
    }
}