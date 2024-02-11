using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Whenever.Core.CommandInitiators;
using Whenever.Core.WheneverTestDemo;
using Whenever.Core.WorldInterface;

namespace Whenever.Core.Effects
{
    public abstract record EffectInitiatorEffect : IEffect<IInspectableWorldDemo, ICommandableWorldDemo>
    {
        public IEnumerable<IWorldCommand<ICommandableWorldDemo>> ApplyEffect(InitiatedCommand<ICommandableWorldDemo> command, IInspectableWorldDemo world)
        {
            if(!command.initiator.TryAsOrRecursedFrom<CombatantCommandInitiator>(out var initiator))
            {
                Debug.LogWarning($"Initiator effect {GetType().Name} can only apply damage on a combatant command initiator");
                return Enumerable.Empty<IWorldCommand<ICommandableWorldDemo>>();
            }
            return this.ApplyEffectToInitiator(initiator.Initiator, world);
        }
        
        protected abstract IEnumerable<IWorldCommand<ICommandableWorldDemo>> ApplyEffectToInitiator(CombatantId initiator, IInspectableWorldDemo world);
    }
}