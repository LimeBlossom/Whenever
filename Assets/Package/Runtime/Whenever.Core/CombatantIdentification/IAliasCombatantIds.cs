using System.Collections.Generic;

public interface IAliasCombatantIds
{
    public CombatantId GetIdForAlias(CombatantAlias alias);
    public IEnumerable<CombatantAlias> AllDefinedAliases();
}