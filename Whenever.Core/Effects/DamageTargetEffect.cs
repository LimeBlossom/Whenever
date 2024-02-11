using System.Collections.Generic;
using UnityEngine;
using WheneverAbstractions._Project.WheneverAbstractions.Commands;

namespace WheneverAbstractions._Project.WheneverAbstractions.Effects
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