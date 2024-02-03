using System;
using System.Collections.Generic;
using UnityEngine;

public enum CombatantType
{
    Player,
    Enemy
}

public interface ICombatantData
{
    public float GetCurrentHealth();
    public Vector2 GetPosition();
    public CombatantType GetCombatantType();
}

public class Combatant : ICombatantData
{
    public Health health;
    public Damageable damageable;
    public List<StatusEffect> statusEffects = new();
    public CombatantType combatantType;
    public Vector2 position;

    public Combatant(int maxHealth, CombatantType type)
    {
        this.health = new Health(maxHealth);
        this.damageable = new Damageable();
        this.combatantType = type;
        
        this.position = new Vector2();
    }
    
    public void StartTurn()
    {
        throw new NotImplementedException();
        // foreach(StatusEffect statusEffect in statusEffects.ToArray())
        // {
        //     var statusEffectResult = statusEffect.ActivateOn(this);
        //     if (statusEffectResult.completion == StatusEffectCompletion.Expired)
        //     {
        //         statusEffects.Remove(statusEffect);
        //     }
        // }
    }

    public float GetCurrentHealth()
    {
        return health.GetCurrentHealth();
    }

    public Vector2 GetPosition()
    {
        return position;
    }

    public CombatantType GetCombatantType()
    {
        return combatantType;
    }
}
