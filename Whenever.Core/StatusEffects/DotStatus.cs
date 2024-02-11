﻿using System.Collections.Generic;
using System.Linq;
using Whenever.Core.CommandInitiators;
using Whenever.Core.Commands;

namespace Whenever.Core.StatusEffects
{
    public record DotStatus : StatusEffect
    {
        public DamagePackage damagePackage;
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
                    commands = Enumerable.Empty<IWorldCommand>()
                };
            }

            var damageCommand = new DamageCommand(target, damagePackage);

            return new StatusEffectResult()
            {
                completion = StatusEffectCompletion.Active,
                commands = new List<IWorldCommand> { damageCommand }
            };
        }

    }
}