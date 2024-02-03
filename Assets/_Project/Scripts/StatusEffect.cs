using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect
{
    public int turnsLeft;
    public float damage;
    // DamageType?
    public Combatant self;
    public Combatant originator;

    public abstract void Activate();
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
