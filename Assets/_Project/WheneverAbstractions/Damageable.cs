using System;
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

public interface IWorldCommand
{
    public CombatantId Target { get; }
}

public interface ICommandInitiator
{
    
}

public record CombatantCommandInitiator: ICommandInitiator
{
    public CombatantId Initiator { get; set; }
}

public struct DamagePackage
{
    public DamageType damageType;
    public float damageAmount;
    
    public DamagePackage(DamageType damageType, float damageAmount)
    {
        this.damageType = damageType;
        this.damageAmount = damageAmount;
    }
}

public struct DamageContext
{
    public DamageSource attacker;
    public ICombatantData target;
}

public enum DamageSourceType
{
    Combatant,
    Effect
}
public struct DamageSource
{
    public DamageSourceType sourceType;
    private ICombatantData attacker;
    private int effectRecursionDepth;
    
    public ICombatantData GetAttacker()
    {
        if (sourceType != DamageSourceType.Combatant) throw new InvalidOperationException();
        return attacker;
    }
    public int GetEffectData()
    {
        if (sourceType != DamageSourceType.Effect) throw new InvalidOperationException();
        return effectRecursionDepth;
    }
}

public interface IDamageable
{
    public DamagePackage ApplyResistances(DamagePackage damagePackage);
}

public class Damageable : IDamageable
{
    private List<DamageResistance> damageResistances;

    public Damageable()
    {
        damageResistances = new List<DamageResistance>();
    }
    
    public DamagePackage ApplyResistances(DamagePackage damagePackage)
    {
        float damageResistance = FindResistanceOfType(damagePackage.damageType).resistance;

        float damage = damagePackage.damageAmount * (1 - damageResistance);
        
        damagePackage.damageAmount = damage;
        return damagePackage;
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
