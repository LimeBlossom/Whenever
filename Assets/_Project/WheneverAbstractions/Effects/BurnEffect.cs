using System.Collections.Generic;
using WheneverAbstractions._Project.WheneverAbstractions.Commands;
using WheneverAbstractions._Project.WheneverAbstractions.StatusEffects;

namespace WheneverAbstractions._Project.WheneverAbstractions.Effects
{
    public class BurnEffect: IEffect
    {
        public IEnumerable<IWorldCommand> ApplyEffect(CombatantId triggerTarget)
        {
            // Apply burn status effect to target
            BurnStatus burnStatus = new();
            burnStatus.damage = 1;
            burnStatus.turnsLeft = 3;
            yield return new AddStatusEffectCommand(triggerTarget, burnStatus);
        }
    }
}