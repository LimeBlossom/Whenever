using System.Collections.Generic;
using UnityEngine;
using WheneverAbstractions._Project.WheneverAbstractions.Commands;

namespace WheneverAbstractions._Project.WheneverAbstractions.Effects
{
    public record DamageAdjacentToTargetEffect: EffectTargetEffect
    {
        public float damageAmount;
        public DamageType damageType;
        protected override IEnumerable<IWorldCommand> ApplyEffectToTarget(CombatantId target, IInspectableWorld world)
        {
            var newTargets = world.GetAdjacentCombatants(target);
            
            foreach (var adjacentTarget in newTargets)
            {
                var damagePackage = new DamagePackage(damageType, damageAmount);
                yield return new DamageCommand(adjacentTarget, damagePackage);
            }
        }
    }
}