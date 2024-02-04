using System.Collections.Generic;
using UnityEngine;
using WheneverAbstractions._Project.WheneverAbstractions.Commands;

namespace WheneverAbstractions._Project.WheneverAbstractions.Effects
{
    public record DamageAdjacentToTargetEffect: IEffect
    {
        public float damageAmount;
        public DamageType damageType;
        public IEnumerable<IWorldCommand> ApplyEffect(InitiatedCommand command, IInspectableWorld world)
        {
            if (command.command is not ITargetedWorldCommand targetedCommand)
            {
                Debug.LogWarning("Target effect can only apply on commands that target a combatant");
                yield break;
            }
            
            var newTargets = world.GetAdjacentCombatants(targetedCommand.Target);
            
            foreach (var target in newTargets)
            {
                var damagePackage = new DamagePackage(damageType, damageAmount);
                yield return new DamageCommand(target, damagePackage);
            }
        }
    }
}