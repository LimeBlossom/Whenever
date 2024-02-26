using System.Collections.Generic;

public class DescribeWithAliases: IDescriptionContext
{
    private readonly IDescribeCombatants combatantDescriber;
    private readonly IAliasCombatantIds aliaser;
    private readonly Dictionary<CombatantAlias, string> aliasNames = new();
    
    public DescribeWithAliases(
        IDescribeCombatants combatantDescriber,
        IAliasCombatantIds aliaser,
        string initiatorName = "the initiator",
        string targetName = "the target")
    {
        this.combatantDescriber = combatantDescriber;
        this.aliaser = aliaser;
        this.aliasNames[StandardAliases.Initiator] = initiatorName;
        this.aliasNames[StandardAliases.Target] = targetName;
    }
    public string NameOf(CombatantId id)
    {
        return combatantDescriber.NameOf(id);
    }

    public string NameOf(CombatantAlias alias)
    {
        if(aliasNames.TryGetValue(alias, out var name))
        {
            return name;
        }
        var idFromUnderlying = aliaser.GetIdForAlias(alias);
        if(idFromUnderlying == null)
        {
            return alias.ReadableDescription;
        }
        return combatantDescriber.NameOf(idFromUnderlying);
    }

    public IAliasCombatantIds GetInternalAliaser()
    {
        return aliaser;
    }
}