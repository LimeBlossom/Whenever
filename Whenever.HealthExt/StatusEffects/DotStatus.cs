﻿using System.Collections.Generic;
using System.Linq;

public record DotStatus : StatusEffect<ICommandWorldHealth>
{
    public float damage;
    public DotStatus(int turnsLeft, ICommandInitiator initiator) : base(turnsLeft, initiator)
    {
    }
    public override StatusEffectResult<ICommandWorldHealth> ActivateOn(CombatantId target)
    {
        if (NextTurnIsExpired())
        {
            return new StatusEffectResult<ICommandWorldHealth>
            {
                completion = StatusEffectCompletion.Expired,
                commands = Enumerable.Empty<IWorldCommand<ICommandWorldHealth>>()
            };
        }

        var damageCommand = new Damage(target, damage);

        return new StatusEffectResult<ICommandWorldHealth>()
        {
            completion = StatusEffectCompletion.Active,
            commands = new List<IWorldCommand<ICommandWorldHealth>> { damageCommand }
        };
    }

}