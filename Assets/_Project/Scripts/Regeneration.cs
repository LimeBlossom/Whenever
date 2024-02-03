using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regeneration : StatusEffect
{
    public override void Activate()
    {
        if (turnsLeft > 0)
        {
            turnsLeft--;

            DamagePackage damagePackage = new();
            damagePackage.damageAmount = -damage;
            damagePackage.damageType = DamageType.HEAL;
            damagePackage.attacker = originator;
            damagePackage.target = self;

            self.damageable.TakeDamage(ref damagePackage);
        }
    }
}
