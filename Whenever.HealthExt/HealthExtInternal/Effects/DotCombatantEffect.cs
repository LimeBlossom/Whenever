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
        public IEnumerable<IWorldCommand<ICommandWorldHealth>> ApplyEffect(
            InitiatedCommand<ICommandWorldHealth> command,
            IAliasCombatantIds aliaser,
            IInspectWorldHealth world)
        {
            if (command.command is not IGenericTargetedWorldCommand<ICommandWorldHealth> targetedCommand)
            {
                Debug.LogWarning("Dot effect can only apply on commands that target a combatant");
                yield break;
            }

            var status = new DotStatus(turns, command.initiator)
            {
                damage = damage
            };
            yield return new AddStatusEffectCommand<ICommandWorldHealth>(targetedCommand.Target, status);
        }

        public string Describe(IDescriptionContext context)
        {
            return $"apply {damage} damage per turn for {turns} turns{context.ToTargetAsDirectSubject()}";
        }
    }
}