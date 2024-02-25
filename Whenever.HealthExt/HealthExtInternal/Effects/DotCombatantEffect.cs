using System.Collections.Generic;
using Serialization;
using UnityEngine;

namespace HealthExtInternal
{
    internal record DotCombatantEffect: IEffect<IInspectWorldHealth, ICommandWorldHealth>
    {
        public float damage;
        public int turns;
        public readonly CombatantAlias alias;

        public DotCombatantEffect(CombatantAlias alias)
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
            var status = new DotStatus(turns, command.initiator)
            {
                damage = damage
            };
            yield return new AddStatusEffectCommand<ICommandWorldHealth>(target, status);
        }

        public string Describe(IDescriptionContext context)
        {
            return $"apply {damage} damage per turn for {turns} turns{context.ToAliasAsDirectSubject(alias)}";
        }
    }
}