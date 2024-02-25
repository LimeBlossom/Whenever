using System.Collections.Generic;

public class SimpleDescriptionContext : IDescriptionContext
{
    public SimpleDescriptionContext(Dictionary<CombatantId, string> name) : this("the initiator", "the target", name)
    {
    }
    public SimpleDescriptionContext(): this("the initiator", "the target")
    {
    }
    
    public SimpleDescriptionContext(string initiatorName, string targetName)
    {
        this.InitiatorName = initiatorName;
        this.TargetName = targetName;
        this.aliaser = new SimpleCombatantAliaser();
    }

    public SimpleDescriptionContext(string initiatorName, string targetName, Dictionary<CombatantId, string> names)
    {
        this.InitiatorName = initiatorName;
        this.TargetName = targetName;
        this.names = names ?? new();
        this.aliaser = new SimpleCombatantAliaser();
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
        var idForAlias = aliaser.GetIdForAlias(alias);
        return idForAlias == null ? alias.ReadableDescription : NameOf(idForAlias);
    }
}