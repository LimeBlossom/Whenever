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

    public DamagePackage(DamageType damageType, float damageAmount, Combatant attacker, Combatant target)
    {
        this.damageType = damageType;
        this.damageAmount = damageAmount;
        this.attacker = attacker;
        this.target = target;
    }
}

public interface IDamageable
{
    public void TakeDamage(ref DamagePackage damagePackage);
}

public class Damageable : MonoBehaviour, IDamageable
{
    [SerializeField] private DamageResistance[] damageResistances;

    public void TakeDamage(ref DamagePackage damagePackage)
    {
        print($"{gameObject.name} took {damagePackage.damageAmount} {damagePackage.damageType} damage");
        float damageResistance = FindResistanceOfType(damagePackage.damageType).resistance;

        float damage = damagePackage.damageAmount * (1 - damageResistance);
        if(damage > 0)
        {
            damagePackage.target.health.Reduce(damage);
        }
        else if(damage < 0)
        {
            damagePackage.target.health.Increase(-damage);
        }
        damagePackage.attacker.wheneverManager.CheckWhenevers(damagePackage, damagePackage.attacker);
        damagePackage.target.wheneverManager.CheckWhenevers(damagePackage, damagePackage.target);
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
