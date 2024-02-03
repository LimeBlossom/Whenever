using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnAction
{
    public void Effect(DamagePackage damagePackage, Combatant owner)
    {
        // Apply burn status effect to target
        Burn burn = new();
        burn.damage = 1;
        burn.combatant = damagePackage.target;
        burn.turnsLeft = 3;
        burn.combatant.statusEffects.Add(burn);
    }
}

public class Burn : StatusEffect
{
    public override bool Activate()
    {
        if (turnsLeft > 0)
        {
            turnsLeft--;

            DamagePackage damagePackage = new();
            damagePackage.damageAmount = damage;
            damagePackage.damageType = DamageType.BURN;
            damagePackage.attacker = combatant;
            damagePackage.target = combatant;

            combatant.damageable.TakeDamage(ref damagePackage);
            combatant.wheneverManager.CheckWhenevers(damagePackage, combatant);
        }

        return IsExpired();
    }
}
