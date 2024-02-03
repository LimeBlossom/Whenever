using System.Collections.Generic;
using UnityEngine;
using WheneverAbstractions._Project.WheneverAbstractions.Commands;
using WheneverAbstractions._Project.WheneverAbstractions.StatusEffects;

namespace WheneverAbstractions._Project.WheneverAbstractions.Effects
{
    public class BurnTargetEffect: IEffect
    {
        public IEnumerable<IWorldCommand> ApplyEffect(InitiatedCommand command, IInspectableWorld world)
        {
            if (command.command is not ITargetedWorldCommand targetedCommand)
            {
                Debug.LogWarning("Burn Target effect can only apply on commands that target at least one combatant");
                yield break;
            }

            // Apply burn status effect to target
            BurnStatus burnStatus = new();
            burnStatus.damage = 1;
            burnStatus.turnsLeft = 3;
            yield return new AddStatusEffectCommand(targetedCommand.Target, burnStatus);
        }
    }
}