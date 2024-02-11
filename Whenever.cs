using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Target
{
    ATTACKER,
    TARGET
}

[Serializable]
public class Whenever
{
    [SerializeField] private DamageType trigger;
    public string effectName;
    [SerializeField] private Target target;
    [SerializeField] private int maxDistance;

    public delegate void Effect(DamagePackage damagePackage, Combatant owner, Target target);
    public Effect effect;

    public void SetTrigger(DamageType trigger)
    {
        this.trigger = trigger;
    }

    public void SetTarget(Target target)
    {
        this.target = target;
    }

    public void TryTrigger(DamagePackage damagePackage, Combatant owner)
    {
        if(damagePackage.damageType == trigger)
          effect?.Invoke(damagePackage, owner, target);
        // if success then WheneverManager.CheckWhenevers?
    }
}
