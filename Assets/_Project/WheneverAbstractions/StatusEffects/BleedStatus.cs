using System.Collections.Generic;
using System.Linq;
using WheneverAbstractions._Project.WheneverAbstractions.Commands;

namespace WheneverAbstractions._Project.WheneverAbstractions.StatusEffects
{
    public record BleedStatus : StatusEffect
    {
        public float damage;
        public BleedStatus(int turnsLeft, ICommandInitiator initiator) : base(turnsLeft, initiator)
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

            DamagePackage damagePackage = new();
            damagePackage.damageAmount = damage;
            damagePackage.damageType = DamageType.BLEED;
            var damageCommand = new DamageCommand(target, damagePackage);

            return new StatusEffectResult()
            {
                completion = StatusEffectCompletion.Active,
                commands = new List<IWorldCommand> { damageCommand }
            };
        }

    }
}