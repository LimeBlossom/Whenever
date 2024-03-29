using System.Collections.Generic;
using UnityEngine;
using Whenever.DmgTypeEtcExt.Experimental.Commands;
using Whenever.DmgTypeEtcExt.Experimental.World;

namespace Whenever.DmgTypeEtcExt.Experimental.Effects
{
    public record DamageAdjacentToCombatantEffect: EffectAliasedTargetEffect<IInspectableWorldDemo, ICommandableWorldDemo>
    {
        public float damageAmount;
        public DamageType damageType;

        public DamageAdjacentToCombatantEffect(CombatantAlias alias) : base(alias)
        {
        }
        protected override IEnumerable<IWorldCommand<ICommandableWorldDemo>> ApplyEffectToTarget(
            CombatantId target,
            InitiatedCommand<ICommandableWorldDemo> command,
            IInspectableWorldDemo world)
        {
            var newTargets = world.GetAdjacentCombatants(target);
            foreach (var adjacentTarget in newTargets)
            {
                var damagePackage = new DamagePackage(damageType, damageAmount);
                yield return new DamageCommand(adjacentTarget, damagePackage);
            }
        }
        
        protected override string DescribeOnTarget()
        {
            return $"deal {damageAmount} {damageType} damage to all adjacent";
        }
    }
}