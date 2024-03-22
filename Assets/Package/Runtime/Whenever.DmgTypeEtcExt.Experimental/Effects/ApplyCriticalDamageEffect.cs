using System.Collections.Generic;
using UnityEngine;
using Whenever.DmgTypeEtcExt.Experimental.Commands;
using Whenever.DmgTypeEtcExt.Experimental.World;

namespace Whenever.DmgTypeEtcExt.Experimental.Effects
{
    public record ApplyCriticalDamageEffect: EffectAliasedTargetEffect<IInspectableWorldDemo, ICommandableWorldDemo>
    {
        public float critDamageMultiplier;

        public ApplyCriticalDamageEffect(CombatantAlias alias) : base(alias)
        {
        }
        
        protected override IEnumerable<IWorldCommand<ICommandableWorldDemo>> ApplyEffectToTarget(
            CombatantId target,
            InitiatedCommand<ICommandableWorldDemo> command,
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
            yield return new DamageCommand(target, damagePackage);
        }

        protected override string DescribeOnTarget()
        {
            return $"apply {critDamageMultiplier}x {DamageType.CRITICAL} damage";
        }
    }
}