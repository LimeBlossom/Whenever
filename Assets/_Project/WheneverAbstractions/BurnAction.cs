using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnAction
{
    public void Effect(DamagePackage damagePackage, Combatant triggerTarget)
    {
        // Apply burn status effect to target
        Burn burn = new();
        burn.damage = 1;
        burn.turnsLeft = 3;
        triggerTarget.statusEffects.Add(burn);
    }
}

public class Burn : StatusEffect
{
    public override StatusEffectResult ActivateOn(Combatant combatant)
    {
        if (IsExpired())
        {
            return new StatusEffectResult
            {
                completion = StatusEffectCompletion.Expired,
                change = StatusEffectChange.NoChange
            };
        }
        turnsLeft--;

        DamagePackage damagePackage = new();
        damagePackage.damageAmount = damage;
        damagePackage.damageType = DamageType.BURN;
        damagePackage.attacker = combatant;
        damagePackage.target = combatant;

        combatant.damageable.TakeDamage(ref damagePackage);

        return new StatusEffectResult
        {
            completion = StatusEffectCompletion.Active,
            change = StatusEffectChange.Changed
        };
    }
}
