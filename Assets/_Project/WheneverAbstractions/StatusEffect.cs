using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatusEffectCompletion
{
    Expired,
    Active
}

public struct StatusEffectResult
{
    public StatusEffectCompletion completion;
    public IEnumerable<IWorldCommand> commands;
}

public abstract class StatusEffect
{
    public int turnsLeft;
    public float damage;

    /// <summary>
    /// returns true when 
    /// </summary>
    /// <returns></returns>
    public abstract StatusEffectResult ActivateOn(CombatantId target);
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