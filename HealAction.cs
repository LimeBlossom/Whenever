using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAction
{
    public int healAmount;

    public void Effect(DamagePackage damagePackage, Combatant owner, Target target)
    {
        DamagePackage damagePack = new();
        damagePack.damageType = DamageType.HEAL;
        damagePack.damageAmount = -healAmount;

        if (target == Target.ATTACKER)
        {
            damagePack.attacker = damagePackage.attacker;
            damagePack.target = damagePackage.attacker;
            damagePack.attacker.damageable.TakeDamage(ref damagePack);
        }
        if (target == Target.TARGET)
        {
            damagePack.attacker = damagePackage.target;
            damagePack.target = damagePackage.target;
            damagePack.attacker.damageable.TakeDamage(ref damagePack);
        }
    }
}
