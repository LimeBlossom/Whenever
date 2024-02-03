using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CombatantType
{
    Player,
    Enemy
}

public class Combatant
{
    public Health health;
    public Damageable damageable;
    public List<StatusEffect> statusEffects = new();
    public CombatantType combatantType;

    public Combatant(int maxHealth, CombatantType type)
    {
        this.health = new Health(maxHealth);
        this.damageable = new Damageable();
        this.combatantType = type;
    }
    
    public void StartTurn()
    {
        foreach(StatusEffect statusEffect in statusEffects.ToArray())
        {
            var statusEffectResult = statusEffect.ActivateOn(this);
            if (statusEffectResult.completion == StatusEffectCompletion.Expired)
            {
                statusEffects.Remove(statusEffect);
            }
        }
    }
}
