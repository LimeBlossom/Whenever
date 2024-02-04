using System.Collections.Generic;
using UnityEngine;
using WheneverAbstractions._Project.WheneverAbstractions.Commands;

namespace WheneverAbstractions._Project.WheneverAbstractions.Effects
{
    public record DamageTargetEffect: IEffect
    {
        public DamagePackage damagePackage;
        public IEnumerable<IWorldCommand> ApplyEffect(InitiatedCommand command, IInspectableWorld world)
        {
            if (command.command is not ITargetedWorldCommand targetedCommand)
            {
                Debug.LogWarning("Damage target effect can only apply on commands that target at least one combatant");
                yield break;
            }
            
            // Apply damage to target
            yield return new DamageCommand(targetedCommand.Target, damagePackage);
        }
    }
}