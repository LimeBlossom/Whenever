﻿using System.Collections.Generic;
using System.Linq;
using Whenever.Core;
using Whenever.Core.CommandInitiators;
using Whenever.Core.WorldInterface;
using Whenever.HealthExt.Commands;
using Whenever.HealthExt.World;

namespace Whenever.HealthExt.StatusEffects
{
    public record DotStatus : StatusEffect
    {
        public float damage;
        public DotStatus(int turnsLeft, ICommandInitiator initiator) : base(turnsLeft, initiator)
        {
        }
        public override StatusEffectResult ActivateOn(CombatantId target)
        {
            if (NextTurnIsExpired())
            {
                return new StatusEffectResult
                {
                    completion = StatusEffectCompletion.Expired,
                    commands = Enumerable.Empty<IWorldCommand<ICommandWorldHealth>>()
                };
            }

            var damageCommand = new Damage(target, damage);

            return new StatusEffectResult()
            {
                completion = StatusEffectCompletion.Active,
                commands = new List<IWorldCommand<ICommandWorldHealth>> { damageCommand }
            };
        }

    }
}