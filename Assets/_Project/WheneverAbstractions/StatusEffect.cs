using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatusEffectCompletion
{
    Expired,
    Active
}

public enum StatusEffectChange
{
    NoChange,
    Changed
}

public struct StatusEffectResult
{
    public StatusEffectCompletion completion;
    public StatusEffectChange change;
}

public abstract class StatusEffect
{
    public int turnsLeft;
    public float damage;

    /// <summary>
    /// returns true when 
    /// </summary>
    /// <returns></returns>
    public abstract StatusEffectResult ActivateOn(Combatant combatant);
    public virtual void SetDamage(float value)
    {
        damage = value;
    }

    public virtual void SetTurnsLeft(int value)
    {
        turnsLeft = value;
    }

    public virtual bool IsExpired()
    {
        if(turnsLeft <= 0)
        {
            return true;
        }
        return false;
    }
}
