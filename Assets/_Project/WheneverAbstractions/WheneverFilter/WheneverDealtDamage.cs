using System;
using WheneverAbstractions._Project.WheneverAbstractions.CommandInitiators;
using WheneverAbstractions._Project.WheneverAbstractions.Commands;

namespace WheneverAbstractions._Project.WheneverAbstractions.WheneverFilter
{
    public class WheneverDealtDamage : IWheneverFilter
    {
        public DamageType validDamageType;
        public WheneverCombatantTypeFilter wheneverCombatantTypeFilterType;

        private bool CanTrigger(DamageCommand damageCommand, ICombatantData triggerTarget, CombatantId triggerTargetId)
        {
            if(damageCommand.Target != triggerTargetId) return false;
            if(damageCommand.damagePackage.damageType != validDamageType) return false;
        
            var targetEnumType = triggerTarget.GetCombatantType() switch
            {
                CombatantType.Player => WheneverCombatantTypeFilter.Player,
                CombatantType.Enemy => WheneverCombatantTypeFilter.Enemy,
                _ => throw new ArgumentOutOfRangeException()
            };
            if((wheneverCombatantTypeFilterType & targetEnumType) == 0) return false;
            return true;
        }

        public bool TriggersOn(IWorldCommand command, ICommandInitiator initiator, GlobalCombatWorld world)
        {
            if(command is DamageCommand damageCommand)
            {
                if(initiator is CombatantCommandInitiator combatantCommandInitiator)
                {
                    var combatantData = world.CombatantData(combatantCommandInitiator.Initiator);
                    return CanTrigger(damageCommand, combatantData, damageCommand.Target);
                }
            }
            throw new NotImplementedException();
        }
    }
}