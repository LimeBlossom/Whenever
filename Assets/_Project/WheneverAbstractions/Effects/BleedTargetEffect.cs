

using System.Collections.Generic;
using UnityEngine;
using WheneverAbstractions._Project.WheneverAbstractions.Commands;
using WheneverAbstractions._Project.WheneverAbstractions.StatusEffects;

namespace WheneverAbstractions._Project.WheneverAbstractions.Effects
{
    public class BleedTargetEffect: IEffect
    {
        public float bleedDamage;
        public int turns;
        public IEnumerable<IWorldCommand> ApplyEffect(InitiatedCommand command, IInspectableWorld world)
        {
            if (command.command is not ITargetedWorldCommand targetedCommand)
            {
                Debug.LogWarning("Bleed Target effect can only apply on commands that target at least one combatant");
                yield break;
            }

            // Apply burn status effect to target
            var bleedStatus = new BleedStatus(turns)
            {
                damage = bleedDamage
            };
            yield return new AddStatusEffectCommand(targetedCommand.Target, bleedStatus);
        }
    }
}