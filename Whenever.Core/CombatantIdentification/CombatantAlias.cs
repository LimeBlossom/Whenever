using System;
using UnityEngine;

[Serializable]
public class CombatantAlias
{
    [SerializeField]
    private string alias;
    [SerializeField]
    private string readableDescription;
    
    public static CombatantAlias FromId(string alias)
    {
        return new CombatantAlias(alias);
    }
    
    public static CombatantAlias FromId(string alias, string readableDescription)
    {
        return new CombatantAlias(alias, readableDescription);
    }
    
    private CombatantAlias(string alias) : this(alias, alias)
    {
    }
    
    private CombatantAlias(string alias, string readableDescription)
    {
        this.alias = alias;
        this.readableDescription = readableDescription;
    }
    
    public string ReadableDescription => readableDescription;
    
    public static implicit operator CombatantAlias(string alias)
    {
        return new CombatantAlias(alias, alias);
    } 

    public override bool Equals(object obj)
    {
        return obj is CombatantAlias other && Equals(other);
    }

    public bool Equals(CombatantAlias other)
    {
        return alias == other.alias;
    }

    public override int GetHashCode()
    {
        return (alias != null ? alias.GetHashCode() : 0);
    }

    public override string ToString()
    {
        return readableDescription;
    }
}