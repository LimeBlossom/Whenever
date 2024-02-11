using System.Collections.Generic;
using Whenever.Core.Commands;

namespace Whenever.Core.Effects
{
    public record DamageTargetEffect: EffectTargetEffect
    {
        public DamagePackage damagePackage;

        protected override IEnumerable<IWorldCommand> ApplyEffectToTarget(CombatantId target, IInspectableWorld world)
        {
            yield return new DamageCommand(target, damagePackage);
        }
    }
}