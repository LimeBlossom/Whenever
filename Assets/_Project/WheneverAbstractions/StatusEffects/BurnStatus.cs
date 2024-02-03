using System.Collections.Generic;
using System.Linq;
using WheneverAbstractions._Project.WheneverAbstractions.Commands;

namespace WheneverAbstractions._Project.WheneverAbstractions.StatusEffects
{
    public class BurnStatus : StatusEffect
    {
        public override StatusEffectResult ActivateOn(CombatantId target)
        {
            if (IsExpired())
            {
                return new StatusEffectResult
                {
                    completion = StatusEffectCompletion.Expired,
                    commands = Enumerable.Empty<IWorldCommand>()
                };
            }
            turnsLeft--;

            DamagePackage damagePackage = new();
            damagePackage.damageAmount = damage;
            damagePackage.damageType = DamageType.BURN;
            var damageCommand = new DamageCommand(target, damagePackage);

            return new StatusEffectResult()
            {
                completion = StatusEffectCompletion.Active,
                commands = new List<IWorldCommand> { damageCommand }
            };
        }
    }
}