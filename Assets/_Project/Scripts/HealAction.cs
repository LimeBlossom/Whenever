using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAction
{
    public int healAmount;

    public void Effect(DamagePackage damagePackage, Combatant owner)
    {
        DamagePackage damagePack = new();
        damagePack.damageType = DamageType.HEAL;
        damagePack.damageAmount = -healAmount;
        damagePack.attacker = owner;
        damagePack.target = owner;

        owner.damageable.TakeDamage(ref damagePack);
    }
}
