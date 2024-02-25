using System.Collections.Generic;
using Serialization;
using UnityEngine;

namespace HealthExtInternal
{
    [PolymorphicSerializable("DotCombatantEffect")]
    internal record DotCombatantEffect: IEffect<IInspectWorldHealth, ICommandWorldHealth>
    {
        public float damage;
        public int turns;
        public CombatantAlias combatant;

        public DotCombatantEffect(CombatantAlias combatant)
        {
            this.combatant = combatant;
        }
        public IEnumerable<IWorldCommand<ICommandWorldHealth>> ApplyEffect(
            InitiatedCommand<ICommandWorldHealth> command,
            IAliasCombatantIds aliaser,
            IInspectWorldHealth world)
        {
            var target = aliaser.GetIdForAlias(combatant);
            if (target == null)
            {
                Debug.LogWarning($"Could not find target for alias '{combatant}'");
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
            return $"apply {damage} damage per turn for {turns} turns{context.ToAliasAsDirectSubject(combatant)}";
        }
    }
}