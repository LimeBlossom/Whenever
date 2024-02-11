using System.Collections.Generic;
using Whenever.Core.Commands;
using Whenever.Core.WheneverTestDemo;
using Whenever.Core.WorldInterface;

namespace Whenever.Core.Effects
{
    public record HealInitiatorEffect: EffectInitiatorEffect
    {
        private float healAmount;

        public HealInitiatorEffect(float healAmount)
        {
            this.healAmount = healAmount;
        }

        protected override IEnumerable<IWorldCommand<ICommandableWorldDemo>> ApplyEffectToInitiator(CombatantId initiator, IInspectableWorldDemo world)
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