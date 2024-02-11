using System.Collections.Generic;
using Whenever.Core.Commands;
using Whenever.Core.WheneverTestDemo;
using Whenever.Core.WorldInterface;

namespace Whenever.Core.Effects
{
    public record DamageTargetEffect: EffectTargetEffect
    {
        public DamagePackage damagePackage;

        protected override IEnumerable<IWorldCommand<ICommandableWorldDemo>> ApplyEffectToTarget(CombatantId target, IInspectableWorldDemo world)
        {
            yield return new DamageCommand(target, damagePackage);
        }
    }
}