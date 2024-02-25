using System.Collections.Generic;
using UnityEngine;
using Whenever.DmgTypeEtcExt.Experimental.Commands;
using Whenever.DmgTypeEtcExt.Experimental.World;

namespace Whenever.DmgTypeEtcExt.Experimental.Effects
{
    public record ApplyCriticalDamageEffect: IEffect<IInspectableWorldDemo, ICommandableWorldDemo>
    {
        public float critDamageMultiplier;
        private readonly CombatantAlias alias;

        public ApplyCriticalDamageEffect(CombatantAlias alias)
        {
            this.alias = alias;
        }
        
        public IEnumerable<IWorldCommand<ICommandableWorldDemo>> ApplyEffect(
            InitiatedCommand<ICommandableWorldDemo> command,
            IAliasCombatantIds aliaser,
            IInspectableWorldDemo world)
        {
            var target = aliaser.GetIdForAlias(alias);
            if (target == null)
            {
                Debug.LogWarning($"Could not find target for alias '{alias}'");
                yield break;
            }
            
            if (command.command is not DamageCommand targetedCommand)
            {
                Debug.LogWarning("apply critical damage effect can only apply on a damage command");
                yield break;
            }
            
            // Apply critical damage to target
            var critAmount = targetedCommand.damagePackage.damageAmount * critDamageMultiplier;
            var damagePackage = new DamagePackage(DamageType.CRITICAL, critAmount);
            yield return new DamageCommand(target, damagePackage);
        }

        public string Describe(IDescriptionContext context)
        {
            return $"apply {critDamageMultiplier}x {DamageType.CRITICAL} damage{context.ToAliasAsDirectSubject(alias)}";
        }
    }
}