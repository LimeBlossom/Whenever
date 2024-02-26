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
        this.InitiatorName = initiatorName;
        this.TargetName = targetName;
        this.names = names ?? new();
        this.aliaser = aliaser ?? new SimpleCombatantAliaser();
    }
    
    private Dictionary<CombatantId, string> names = new();
    public string InitiatorName { get; set; }
    public string TargetName { get; set;  }
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
        if (alias.Equals(StandardAliases.Target)) return TargetName;
        if (alias.Equals(StandardAliases.Initiator)) return InitiatorName;
        var idForAlias = aliaser.GetIdForAlias(alias);
        return idForAlias == null ? alias.ReadableDescription : NameOf(idForAlias);
    }
}