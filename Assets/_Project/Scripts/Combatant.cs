using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Combatant : MonoBehaviour
{
    public Health health;
    public Damageable damageable;
    public List<StatusEffect> statusEffects = new();
    public WheneverManager wheneverManager;

    public void StartTurn()
    {
        foreach(StatusEffect statusEffect in statusEffects.ToArray())
        {
            statusEffect.Activate();
            if(statusEffect.IsExpired())
            {
                statusEffects.Remove(statusEffect);
            }
        }
    }

    public void EndTurn()
    {
        wheneverManager.Clear();
    }
}
