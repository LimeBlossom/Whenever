using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    NONE,
    HEAL,
    PHYSICAL,
    CRITICAL,
    BLEED,
    FIRE,
    BURN,
    POISON
}

public struct DamageResistance
{
    public DamageType damageType;
    public float resistance; // A number between 0 and 1
}

public struct DamagePackage
{
    public DamageType damageType;
    public float damageAmount;
    public Combatant attacker;
    public Combatant target;
}

public struct DamageContext
{
    
}

public interface IDamageable
{
    public void TakeDamage(ref DamagePackage damagePackage);
}

public class Damageable : IDamageable
{
    private List<DamageResistance> damageResistances;

    public Damageable()
    {
        damageResistances = new List<DamageResistance>();
    }
    
    public void TakeDamage(ref DamagePackage damagePackage)
    {
        float damageResistance = FindResistanceOfType(damagePackage.damageType).resistance;

        float damage = damagePackage.damageAmount * (1 - damageResistance);
        damagePackage.target.health.Reduce(damage);
    }

    private DamageResistance FindResistanceOfType(DamageType damageType)
    {
        if(damageResistances == null)
        {
            return new DamageResistance();
        }

        foreach(DamageResistance damageResistance in damageResistances)
        {
            if(damageResistance.damageType == damageType)
            {
                return damageResistance;
            }
        }
        return new DamageResistance();
    }
}
