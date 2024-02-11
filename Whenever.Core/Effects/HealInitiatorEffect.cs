using System.Collections.Generic;
using WheneverAbstractions._Project.WheneverAbstractions.Commands;

namespace WheneverAbstractions._Project.WheneverAbstractions.Effects
{
    public record HealInitiatorEffect: EffectInitiatorEffect
    {
        private float healAmount;

        public HealInitiatorEffect(float healAmount)
        {
            this.healAmount = healAmount;
        }

        protected override IEnumerable<IWorldCommand> ApplyEffectToInitiator(CombatantId initiator, IInspectableWorld world)
        {
            var damagePackage = new DamagePackage
            {
                damageAmount = -healAmount,
                damageType = DamageType.HEAL
            };
            yield return new DamageCommand(initiator, damagePackage);
        }
    }
}