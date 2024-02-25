using System.Collections.Generic;
using UnityEngine;
using Whenever.DmgTypeEtcExt.Experimental.Commands;
using Whenever.DmgTypeEtcExt.Experimental.World;

namespace Whenever.DmgTypeEtcExt.Experimental.Effects
{
    public record DamageAdjacentToCombatantEffect: IEffect<IInspectableWorldDemo, ICommandableWorldDemo>
    {
        public float damageAmount;
        public DamageType damageType;
        private readonly CombatantAlias alias;

        public DamageAdjacentToCombatantEffect(CombatantAlias alias)
        {
            this.alias = alias;
        }
        public IEnumerable<IWorldCommand<ICommandableWorldDemo>> ApplyEffect(
            InitiatedCommand<ICommandableWorldDemo> command,
            IAliasCombatantIds aliaser,
            IInspectableWorldDemo world)
        {
            var target = aliaser.GetIdForAlias(alias);
            if (target == null)
            {
                Debug.LogWarning($"Could not find target for alias '{alias}'");
                yield break;
            }
            
            var newTargets = world.GetAdjacentCombatants(target);
            foreach (var adjacentTarget in newTargets)
            {
                var damagePackage = new DamagePackage(damageType, damageAmount);
                yield return new DamageCommand(adjacentTarget, damagePackage);
            }
        }
        
        public string Describe(IDescriptionContext context)
        {
            return $"deal {damageAmount} {damageType} damage to all adjacent{context.ToAliasAsDirectSubject(alias)}";
        }
    }
}