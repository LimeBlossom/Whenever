using System.Collections.Generic;

public class SimpleCombatantAliaser : IAliasCombatantIds
{
    private readonly Dictionary<CombatantAlias, CombatantId> aliasToId = new();
    
    public SimpleCombatantAliaser()
    {
    }
    
    public CombatantId GetIdForAlias(CombatantAlias alias)
    {
        return aliasToId[alias];
    }

    public void SetAlias(CombatantAlias alias, CombatantId id)
    {
        aliasToId[alias] = id;
    }
    
    public void ClearAlias(CombatantAlias alias)
    {
        aliasToId.Remove(alias);
    }
}