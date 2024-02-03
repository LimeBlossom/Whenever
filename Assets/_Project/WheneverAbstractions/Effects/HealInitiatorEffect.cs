using System.Collections.Generic;
using UnityEngine;
using WheneverAbstractions._Project.WheneverAbstractions.CommandInitiators;
using WheneverAbstractions._Project.WheneverAbstractions.Commands;
using WheneverAbstractions._Project.WheneverAbstractions.StatusEffects;

namespace WheneverAbstractions._Project.WheneverAbstractions.Effects
{
    public record HealInitiatorEffect: IEffect
    {
        private float healAmount;

        public HealInitiatorEffect(float healAmount)
        {
            this.healAmount = healAmount;
        }

        public IEnumerable<IWorldCommand> ApplyEffect(InitiatedCommand command, IInspectableWorld world)
        {
            if (!command.initiator.TryAsOrRecursedFrom<CombatantCommandInitiator>(out var initiator))
            {
                Debug.LogWarning("Heal effect can only be applied to initiators which came from combatants");
                yield break;
            }
            var damagePackage = new DamagePackage
            {
                damageAmount = -healAmount,
                damageType = DamageType.HEAL
            };
            yield return new DamageCommand(initiator.Initiator, damagePackage);
        }
    }
}