using System.Collections.Generic;
using System.Linq;

public class SimpleCombatantAliaser : IAliasCombatantIds
{
    private readonly Dictionary<CombatantAlias, CombatantId> aliasToId = new();
    
    public SimpleCombatantAliaser()
    {
    }
    
    public SimpleCombatantAliaser(params (CombatantAlias, CombatantId)[] aliases)
    {
        foreach (var (alias, id) in aliases)
        {
            SetAlias(alias, id);
        }
    }
    
    public CombatantId GetIdForAlias(CombatantAlias alias)
    {
        return aliasToId.GetValueOrDefault(alias);
    }

    public IEnumerable<CombatantAlias> AllDefinedAliases()
    {
        return aliasToId.Keys;
    }
    
    /// <summary>
    /// Set the alias if <paramref name="id"/> is non-null, otherwise clear the alias.
    /// </summary>
    /// <param name="alias"></param>
    /// <param name="id"></param>
    public void SetOrClearAlias(CombatantAlias alias, CombatantId id)
    {
        if (id == null)
        {
            ClearAlias(alias);
        }
        else
        {
            SetAlias(alias, id);
        }
    }
    
    public void SetAlias(CombatantAlias alias, CombatantId id)
    {
        aliasToId[alias] = id;
    }
    
    public void ClearAlias(CombatantAlias alias)
    {
        aliasToId.Remove(alias);
    }
    
    public SimpleCombatantAliaser Clone()
    {
        return new SimpleCombatantAliaser(
            aliasToId
                .Select(x => (x.Key, x.Value))
                .ToArray()
            );
    }
}

public static class SimpleCombatantAliaserExtensions
{
    /// <summary>
    /// Bakes the currently defined aliases in the aliaser into a new SimpleCombatantAliaser. Effectively is a clone
    /// of the current state, clearing any references to the original aliaser.
    /// </summary>
    /// <param name="aliaser"></param>
    /// <returns></returns>
    public static SimpleCombatantAliaser BakeInto(this IAliasCombatantIds aliaser)
    {
        var newAliaser = new SimpleCombatantAliaser();
        foreach (var alias in aliaser.AllDefinedAliases())
        {
            newAliaser.SetAlias(alias, aliaser.GetIdForAlias(alias));
        }
        return newAliaser;
    }
}