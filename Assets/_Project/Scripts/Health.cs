using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float curHealth;
    [SerializeField] private float maxHealth;

    public void Reduce(float value)
    {
        curHealth -= value;
        CheckForDeath();
    }

    public void Increase(float value)
    {
        curHealth += value;
        if(curHealth > maxHealth)
        {
            curHealth = maxHealth;
        }
    }

    private void CheckForDeath()
    {
        if(curHealth <= 0)
        {
            //Dead
        }
    }
}
