using System.Collections.Generic;
using Serialization;
using UnityEngine;

namespace HealthExtInternal
{
    [PolymorphicSerializable("DamageCombatantEffect")]
    internal record DamageCombatantEffect: EffectAliasedTargetEffect<IInspectWorldHealth, ICommandWorldHealth>
    {
        public float damage;

        public DamageCombatantEffect(CombatantAlias combatant) : base(combatant)
        {
        }
        protected override IEnumerable<IWorldCommand<ICommandWorldHealth>> ApplyEffectToTarget(
            CombatantId target,
            InitiatedCommand<ICommandWorldHealth> command,
            IInspectWorldHealth world)
        {
            yield return new Damage(target, damage);
        }

        protected override string DescribeOnTarget()
        {
            return $"deal {damage} damage";
        }
    }
}