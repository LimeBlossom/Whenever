using System;
using UnityEngine;

public enum Target
{
    EVOKER,
    TARGET,
    ALL
}

[Serializable]
public class Whenever
{
    [SerializeField] private DamageType trigger;
    [SerializeField] private bool onEnemy;
    [SerializeField] private Target target;
    [SerializeField] private int maxDistance;

    public delegate void Effect(DamagePackage damagePackage, Combatant owner);
    public Effect effect;

    public void SetTrigger(DamageType trigger)
    {
        this.trigger = trigger;
    }

    public void TryTrigger(DamagePackage damagePackage, Combatant owner)
    {
        if(damagePackage.damageType == trigger)
            /*newDamagePackage =*/ effect?.Invoke(damagePackage, owner);
    }
}
