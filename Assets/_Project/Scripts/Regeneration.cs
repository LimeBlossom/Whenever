using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regeneration : StatusEffect
{
    public override bool Activate()
    {
        if (turnsLeft > 0)
        {
            turnsLeft--;

            DamagePackage damagePackage = new();
            damagePackage.damageAmount = -damage;
            damagePackage.damageType = DamageType.HEAL;
            damagePackage.attacker = combatant;
            damagePackage.target = combatant;

            combatant.damageable.TakeDamage(ref damagePackage);
            combatant.wheneverManager.CheckWhenevers(damagePackage, combatant);
        }

        return IsExpired();
    }
}
