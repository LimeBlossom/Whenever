﻿using System.Collections.Generic;
using System.Linq;
using WheneverAbstractions._Project.WheneverAbstractions.Commands;

namespace WheneverAbstractions._Project.WheneverAbstractions.StatusEffects
{
    public class BurnStatus : StatusEffect
    {
        public float damage;
        public BurnStatus(int turnsLeft) : base(turnsLeft)
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

            DamagePackage damagePackage = new()
            {
                damageAmount = damage,
                damageType = DamageType.BURN
            };
            var damageCommand = new DamageCommand(target, damagePackage);

            return new StatusEffectResult()
            {
                completion = StatusEffectCompletion.Active,
                commands = new List<IWorldCommand> { damageCommand }
            };
        }

    }
}