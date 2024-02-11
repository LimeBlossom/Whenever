using System.Collections.Generic;
using Whenever.Core.Commands;
using Whenever.Core.WheneverTestDemo;
using Whenever.Core.WorldInterface;

namespace Whenever.Core.Effects
{
    public record DamageAdjacentToTargetEffect: EffectTargetEffect
    {
        public float damageAmount;
        public DamageType damageType;
        protected override IEnumerable<IWorldCommand<ICommandableWorldDemo>> ApplyEffectToTarget(CombatantId target, IInspectableWorldDemo world)
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