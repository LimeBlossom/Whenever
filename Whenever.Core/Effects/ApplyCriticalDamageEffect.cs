using System.Collections.Generic;
using UnityEngine;
using Whenever.Core.Commands;
using Whenever.Core.WheneverTestDemo;
using Whenever.Core.WorldInterface;

namespace Whenever.Core.Effects
{
    public record ApplyCriticalDamageEffect: IEffect<IInspectableWorldDemo, ICommandableWorldDemo>
    {
        public float critDamageMultiplier;
        public IEnumerable<IWorldCommand<ICommandableWorldDemo>> ApplyEffect(InitiatedCommand<ICommandableWorldDemo> command, IInspectableWorldDemo world)
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