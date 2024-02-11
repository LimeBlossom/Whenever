using System.Collections.Generic;
using Whenever.Core.Commands;

namespace Whenever.Core.Effects
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