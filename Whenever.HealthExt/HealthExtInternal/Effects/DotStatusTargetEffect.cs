using System.Collections.Generic;
using Serialization;
using UnityEngine;

namespace HealthExtInternal
{
    [PolymorphicSerializable("DotStatusTargetEffect")]
    internal record DotStatusTargetEffect: IEffect<IInspectWorldHealth, ICommandWorldHealth>
    {
        public float damage;
        public int turns;
        public IEnumerable<IWorldCommand<ICommandWorldHealth>> ApplyEffect(InitiatedCommand<ICommandWorldHealth> command, IInspectWorldHealth world)
        {
            if (command.command is not IGenericTargetedWorldCommand<ICommandWorldHealth> targetedCommand)
            {
                Debug.LogWarning("Burn Target effect can only apply on commands that target at least one combatant");
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