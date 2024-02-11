using System.Collections.Generic;
using Whenever.Core;
using Whenever.Core.Effects;
using Whenever.Core.WorldInterface;
using Whenever.DmgTypeEtcExt.Experimental.Commands;
using Whenever.DmgTypeEtcExt.Experimental.World;

namespace Whenever.DmgTypeEtcExt.Experimental.Effects
{
    public record DamageAdjacentToTargetEffect: EffectTargetEffect<IInspectableWorldDemo, ICommandableWorldDemo>
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