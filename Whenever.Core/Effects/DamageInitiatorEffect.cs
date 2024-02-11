using System.Collections.Generic;
using Whenever.Core.Commands;

namespace Whenever.Core.Effects
{
    public record DamageInitiatorEffect: EffectInitiatorEffect
    {
        public DamagePackage damagePackage;
        protected override IEnumerable<IWorldCommand> ApplyEffectToInitiator(CombatantId initiator, IInspectableWorld world)
        {
            yield return new DamageCommand(initiator, damagePackage);
        }
    }
}