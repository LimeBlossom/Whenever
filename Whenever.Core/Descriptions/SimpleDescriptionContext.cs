using System.Collections.Generic;

public class SimpleDescriptionContext : IDescribeCombatants
{
    private SimpleDescriptionContext(Dictionary<CombatantId, string> names = null)
    {
        this.names = names ?? new();
    }

    public static IDescriptionContext CreateInstance(
        string initiatorName = "the initiator",
        string targetName = "the target",
        Dictionary<CombatantId, string> names = null,
        IAliasCombatantIds aliaser = null)
    {
        aliaser ??= new SimpleCombatantAliaser();
        var combatantDescriber = new SimpleDescriptionContext(names);
        var withAliases = new DescribeWithAliases(combatantDescriber, aliaser, initiatorName, targetName);
        return withAliases;
    }

    private Dictionary<CombatantId, string> names;
    public string NameOf(CombatantId id)
    {
        if(names.TryGetValue(id, out var name))
        {
            return name;
        }
        return "Combatant #" + id.ToString();
    }
}