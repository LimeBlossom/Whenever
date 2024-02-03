using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Project.WheneverAbstractions;
using UnityEngine;

public class BurnAction: IEffect
{
    public IEnumerable<IWorldCommand> ApplyEffect(CombatantId triggerTarget)
    {
        // Apply burn status effect to target
        Burn burn = new();
        burn.damage = 1;
        burn.turnsLeft = 3;
        yield return new AddStatusEffectCommand(triggerTarget, burn);
    }
}

public class Burn : StatusEffect
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
        var damageCommand = new DamageCommand()
        {
            damagePackage = damagePackage,
            Target = target
        };

        return new StatusEffectResult()
        {
            completion = StatusEffectCompletion.Active,
            commands = new List<IWorldCommand> { damageCommand }
        };
    }
}
