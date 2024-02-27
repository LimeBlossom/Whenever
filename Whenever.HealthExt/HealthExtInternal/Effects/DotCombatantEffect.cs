using System.Collections.Generic;
using Serialization;
using UnityEngine;

namespace HealthExtInternal
{
    [PolymorphicSerializable("DotCombatantEffect")]
    internal record DotCombatantEffect: EffectAliasedTargetEffect<IInspectWorldHealth, ICommandWorldHealth>
    {
        public float damage;
        public int turns;

        public DotCombatantEffect(CombatantAlias combatant) : base(combatant)
        {
        }
        protected override IEnumerable<IWorldCommand<ICommandWorldHealth>> ApplyEffectToTarget(
            CombatantId target,
            InitiatedCommand<ICommandWorldHealth> command,
            IInspectWorldHealth world)
        {
            var status = new DotStatus(turns, command.initiator)
            {
                damage = damage
            };
            yield return new AddStatusEffectCommand<ICommandWorldHealth>(target, status);
        }

        protected override string DescribeOnTarget()
        {
            return $"apply {damage} damage per turn for {turns} turns";
        }
    }
}