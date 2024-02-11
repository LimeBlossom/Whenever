using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedAction
{
    public void Effect(DamagePackage damagePackage, Combatant owner, Target target)
    {
        // Apply bleed status effect to target
        Bleed bleed = new();
        if (target == Target.TARGET)
        {
            bleed.self = damagePackage.target;
            bleed.originator = damagePackage.attacker;
        }
        if (target == Target.ATTACKER)
        {
            bleed.self = damagePackage.attacker;
            bleed.originator = damagePackage.attacker;
        }
        bleed.damage = 1;
        bleed.turnsLeft = 3;
        damagePackage.target.statusEffects.Add(bleed);
    }
}

public class Bleed : StatusEffect
{
    public override void Activate()
    {
        if (turnsLeft > 0)
        {
            turnsLeft--;

            DamagePackage damagePackage = new();
            damagePackage.damageAmount = damage;
            damagePackage.damageType = DamageType.BLEED;
            damagePackage.attacker = originator;
            damagePackage.target = self;

            self.damageable.TakeDamage(ref damagePackage);
        }
    }
}
