using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WheneverAbstractions._Project.WheneverAbstractions.CommandInitiators;
using WheneverAbstractions._Project.WheneverAbstractions.Commands;

namespace WheneverAbstractions._Project.WheneverAbstractions.Effects
{
    public abstract record EffectInitiatorEffect : IEffect
    {
        public IEnumerable<IWorldCommand> ApplyEffect(InitiatedCommand command, IInspectableWorld world)
        {
            if(!command.initiator.TryAsOrRecursedFrom<CombatantCommandInitiator>(out var initiator))
            {
                Debug.LogWarning($"Initiator effect {GetType().Name} can only apply damage on a combatant command initiator");
                return Enumerable.Empty<IWorldCommand>();
            }
            return this.ApplyEffectToInitiator(initiator.Initiator, world);
        }
        
        protected abstract IEnumerable<IWorldCommand> ApplyEffectToInitiator(CombatantId initiator, IInspectableWorld world);
    }
}