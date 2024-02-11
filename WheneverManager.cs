using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheneverManager : MonoBehaviour
{
    [SerializeField] private List<Whenever> whenevers = new();
    
    public void AddWhenever(Whenever whenever)
    {
        whenevers.Add(whenever);
    }

    public void CheckWhenevers(DamagePackage damagePackage, Combatant owner)
    {
        foreach(Whenever whenever in whenevers)
        {
            whenever.TryTrigger(damagePackage, owner);
        }
    }

    public void Clear()
    {
        whenevers.Clear();
    }
}
