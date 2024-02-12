using System.Collections.Generic;
using UnityEngine;
using Whenever.Core.Commands;
using Whenever.Core.Effects;
using Whenever.Core.WorldInterface;
using Whenever.HealthExt.Commands;
using Whenever.HealthExt.StatusEffects;
using Whenever.HealthExt.World;

namespace Whenever.HealthExt.Effects
{
    public record DotStatusTargetEffect: IEffect<IInspectWorldHealth, ICommandWorldHealth>
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
            yield return new AddStatusEffectCommand(targetedCommand.Target, status);
        }

        public string Describe()
        {
            return $"apply {damage} damage per turn for {turns} turns to the target";
        }
    }
}