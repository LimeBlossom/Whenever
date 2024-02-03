using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health
{
    private float curHealth;
    private float maxHealth;

    public Health(float maxHealth)
    {
        this.maxHealth = maxHealth;
        this.curHealth = maxHealth;
    }
    
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

    public float GetCurrentHealth()
    {
        return curHealth;
    }
    
    private void CheckForDeath()
    {
        if(curHealth <= 0)
        {
            //Dead
        }
    }
}
