using JetBrains.Annotations;

public interface IAliasCombatantIds
{
    public CombatantId GetIdForAlias(CombatantAlias alias);
}