using System.Collections.Generic;
using WheneverAbstractions._Project.WheneverAbstractions.Commands;
using WheneverAbstractions._Project.WheneverAbstractions.StatusEffects;

namespace WheneverAbstractions._Project.WheneverAbstractions.Effects
{
    public class HealEffect: IEffect
    {
        private float healAmount;

        public HealEffect(float healAmount)
        {
            this.healAmount = healAmount;
        }

        public IEnumerable<IWorldCommand> ApplyEffect(CombatantId triggerTarget)
        {
            var damagePackage = new DamagePackage
            {
                damageAmount = -healAmount,
                damageType = DamageType.HEAL
            };
            yield return new DamageCommand()
            {
                damagePackage = damagePackage,
                Target = triggerTarget
            };
        }
    }
}