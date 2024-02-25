using System.Collections.Generic;
using Serialization;
using UnityEngine;

namespace HealthExtInternal
{
    [PolymorphicSerializable("DamageCombatantEffect")]
    internal record DamageCombatantEffect: IEffect<IInspectWorldHealth, ICommandWorldHealth>
    {
        public float damage;
        public CombatantAlias combatant;

        public DamageCombatantEffect(CombatantAlias combatant)
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

            yield return new Damage(target, damage);
        }

        public string Describe(IDescriptionContext context)
        {
            return $"deal {damage} damage{context.ToAliasAsDirectSubject(combatant)}";
        }
    }
}