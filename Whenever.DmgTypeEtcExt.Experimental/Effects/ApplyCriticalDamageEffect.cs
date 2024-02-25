using System.Collections.Generic;
using UnityEngine;
using Whenever.DmgTypeEtcExt.Experimental.Commands;
using Whenever.DmgTypeEtcExt.Experimental.World;

namespace Whenever.DmgTypeEtcExt.Experimental.Effects
{
    public record ApplyCriticalDamageEffect: IEffect<IInspectableWorldDemo, ICommandableWorldDemo>
    {
        public float critDamageMultiplier;
        public IEnumerable<IWorldCommand<ICommandableWorldDemo>> ApplyEffect(
            InitiatedCommand<ICommandableWorldDemo> command,
            IAliasCombatantIds aliaser,
            IInspectableWorldDemo world)
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

        public string Describe(IDescriptionContext context)
        {
            return $"apply {critDamageMultiplier}x {DamageType.CRITICAL} damage to the target";
        }
    }
}