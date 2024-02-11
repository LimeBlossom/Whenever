using System.Collections.Generic;
using Whenever.Core.Commands;

namespace Whenever.Core.Effects
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