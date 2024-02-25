using System.Collections.Generic;
using Serialization;
using UnityEngine;

namespace HealthExtInternal
{
    internal record DamageCombatantEffect: IEffect<IInspectWorldHealth, ICommandWorldHealth>
    {
        public float damage;
        public readonly CombatantAlias alias;

        public DamageCombatantEffect(CombatantAlias alias)
        {
            this.alias = alias;
        }
        public IEnumerable<IWorldCommand<ICommandWorldHealth>> ApplyEffect(
            InitiatedCommand<ICommandWorldHealth> command,
            IAliasCombatantIds aliaser,
            IInspectWorldHealth world)
        {
            var target = aliaser.GetIdForAlias(alias);
            if (target == null)
            {
                Debug.LogWarning($"Could not find target for alias '{alias}'");
                yield break;
            }

            yield return new Damage(target, damage);
        }

        public string Describe(IDescriptionContext context)
        {
            return $"deal {damage} damage{context.ToAliasAsDirectSubject(alias)}";
        }
    }
}