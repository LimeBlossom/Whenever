using System.Collections.Generic;
using Whenever.Core;
using Whenever.Core.Effects;
using Whenever.Core.WorldInterface;
using Whenever.DmgTypeEtcExt.Experimental.Commands;
using Whenever.DmgTypeEtcExt.Experimental.World;

namespace Whenever.DmgTypeEtcExt.Experimental.Effects
{
    public record HealInitiatorEffect: EffectInitiatorEffect<IInspectableWorldDemo, ICommandableWorldDemo>
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