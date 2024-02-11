using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnAction
{
    public void Effect(DamagePackage damagePackage, Combatant owner, Target target)
    {
        // Apply burn status effect to target
        Burn burn = new();
        if (target == Target.TARGET)
        {
            burn.self = damagePackage.target;
            burn.originator = damagePackage.attacker;
        }
        if(target == Target.ATTACKER)
        {
            burn.self = damagePackage.attacker;
            burn.originator = damagePackage.attacker;
        }
        burn.damage = 1;
        burn.turnsLeft = 3;
        damagePackage.target.statusEffects.Add(burn);
    }
}

public class Burn : StatusEffect
{
    public override void Activate()
    {
        if (turnsLeft > 0)
        {
            turnsLeft--;

            DamagePackage damagePackage = new();
            damagePackage.damageAmount = damage;
            damagePackage.damageType = DamageType.BURN;
            damagePackage.attacker = originator;
            damagePackage.target = self;

            self.damageable.TakeDamage(ref damagePackage);
        }
    }
}
