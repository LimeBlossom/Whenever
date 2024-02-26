using System.Collections.Generic;

public class SimpleDescriptionContext : IDescriptionContext
{
    public SimpleDescriptionContext(Dictionary<CombatantId, string> name) : this("the initiator", "the target", name)
    {
    }

    public SimpleDescriptionContext(
        string initiatorName = "the initiator",
        string targetName = "the target",
        Dictionary<CombatantId, string> names = null,
        IAliasCombatantIds aliaser = null)
    {
        this.aliaser = aliaser ?? new SimpleCombatantAliaser();
        this.aliasNames[StandardAliases.Initiator] = initiatorName;
        this.aliasNames[StandardAliases.Target] = targetName;
        this.names = names ?? new();
    }
    
    private Dictionary<CombatantId, string> names = new();
    private Dictionary<CombatantAlias, string> aliasNames = new();
    private IAliasCombatantIds aliaser;
    public string NameOf(CombatantId id)
    {
        if(names.TryGetValue(id, out var name))
        {
            return name;
        }
        return "Combatant #" + id.ToString();
    }
    public string NameOf(CombatantAlias alias)
    {
        if(aliasNames.TryGetValue(alias, out var name))
        {
            return name;
        }
        var idForAlias = aliaser.GetIdForAlias(alias);
        return idForAlias == null ? alias.ReadableDescription : NameOf(idForAlias);
    }

    public IAliasCombatantIds GetInternalAliaser()
    {
        return aliaser;
    }
}