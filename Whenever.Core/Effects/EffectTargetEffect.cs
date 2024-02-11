﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Whenever.Core.Commands;

namespace Whenever.Core.Effects
{
    public abstract record EffectTargetEffect : IEffect
    {
        public IEnumerable<IWorldCommand> ApplyEffect(InitiatedCommand command, IInspectableWorld world)
        {
            if (command.command is not ITargetedWorldCommand targetedCommand)
            {
                Debug.LogWarning($"Target effect {GetType().Name} can only apply on commands that target at least one combatant");
                return Enumerable.Empty<IWorldCommand>();
            }
            return this.ApplyEffectToTarget(targetedCommand.Target, world);
        }
        
        protected abstract IEnumerable<IWorldCommand> ApplyEffectToTarget(CombatantId target, IInspectableWorld world);
    }
}