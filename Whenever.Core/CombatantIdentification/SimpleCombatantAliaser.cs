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