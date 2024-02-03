using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalAction
{
    public void Effect(DamagePackage damagePackage, Combatant owner, Target target)
    {
        damagePackage.damageType = DamageType.CRITICAL;

        damagePackage.target.damageable.TakeDamage(ref damagePackage);
    }
}
