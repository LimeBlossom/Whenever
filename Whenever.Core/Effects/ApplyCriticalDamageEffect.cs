using System.Collections.Generic;
using UnityEngine;
using WheneverAbstractions._Project.WheneverAbstractions.Commands;

namespace WheneverAbstractions._Project.WheneverAbstractions.Effects
{
    public record ApplyCriticalDamageEffect: IEffect
    {
        public float critDamageMultiplier;

        public IEnumerable<IWorldCommand> ApplyEffect(InitiatedCommand command, IInspectableWorld world)
        {
            if (command.command is not DamageCommand targetedCommand)
            {
                Debug.LogWarning("apply critical damage effect can only apply on a damage command");
                yield break;
            }
            
            // Apply critical damage to target
            var critAmount = targetedCommand.damagePackage.damageAmount * critDamageMultiplier;
            var damagePackage = new DamagePackage(DamageType.CRITICAL, critAmount);
            yield return new DamageCommand(targetedCommand.Target, damagePackage);
        }
    }
}