using System.Collections.Generic;
using UnityEngine;
using WheneverAbstractions._Project.WheneverAbstractions.Commands;
using WheneverAbstractions._Project.WheneverAbstractions.StatusEffects;
using WheneverAbstractions._Project.WheneverAbstractions.CommandInitiators;

namespace WheneverAbstractions._Project.WheneverAbstractions.Effects
{
    public record DamageInitiatorEffect: IEffect
    {
        public DamagePackage damagePackage;
        public IEnumerable<IWorldCommand> ApplyEffect(InitiatedCommand command, IInspectableWorld world)
        {
            if(!command.initiator.TryAsOrRecursedFrom<CombatantCommandInitiator>(out var initiator))
            {
                Debug.LogWarning("apply damage can only apply damage on a combatant command initiator");
                yield break;
            }
            
            // Apply damage to initiator
            yield return new DamageCommand(initiator.Initiator, damagePackage);
        }
    }
}