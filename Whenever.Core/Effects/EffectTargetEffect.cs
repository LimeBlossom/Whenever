using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Whenever.Core.Commands;
using Whenever.Core.WheneverTestDemo;
using Whenever.Core.WorldInterface;

namespace Whenever.Core.Effects
{
    public abstract record EffectTargetEffect : IEffect<IInspectableWorldDemo, ICommandableWorldDemo>
    {
        public IEnumerable<IWorldCommand<ICommandableWorldDemo>> ApplyEffect(InitiatedCommand<ICommandableWorldDemo> command, IInspectableWorldDemo world)
        {
            if (command.command is not ITargetedWorldCommand targetedCommand)
            {
                Debug.LogWarning($"Target effect {GetType().Name} can only apply on commands that target at least one combatant");
                return Enumerable.Empty<IWorldCommand<ICommandableWorldDemo>>();
            }
            return this.ApplyEffectToTarget(targetedCommand.Target, world);
        }
        
        protected abstract IEnumerable<IWorldCommand<ICommandableWorldDemo>> ApplyEffectToTarget(CombatantId target, IInspectableWorldDemo world);
    }
}