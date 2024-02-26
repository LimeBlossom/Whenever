using System.Collections.Generic;

public class DescribeWithAliases: IDescriptionContext
{
    public IDescribeCombatants CombatantDescriber { get; }
    public IAliasCombatantIds Aliaser { get; }
    public IReadOnlyDictionary<CombatantAlias, string> AliasFallbackNames { get; }

    private DescribeWithAliases(
        IDescribeCombatants combatantDescriber,
        IAliasCombatantIds aliaser,
        IReadOnlyDictionary<CombatantAlias, string> aliasFallbackNames)
    {
        this.CombatantDescriber = combatantDescriber;
        this.Aliaser = aliaser;
        this.AliasFallbackNames = aliasFallbackNames;
    }

    public static DescribeWithAliases WithStandardAliases(
        IDescribeCombatants combatantDescriber,
        IAliasCombatantIds aliaser,
        string initiatorName = "the initiator",
        string targetName = "the target")
    {
        var fallbackNames = new Dictionary<CombatantAlias, string>();
        fallbackNames[StandardAliases.Initiator] = initiatorName;
        fallbackNames[StandardAliases.Target] = targetName;
        return new DescribeWithAliases(combatantDescriber, aliaser, fallbackNames);
    }

    public string NameOf(CombatantId id)
    {
        return CombatantDescriber.NameOf(id);
    }

    public string TryNameOf(CombatantAlias alias)
    {
        var idFromUnderlying = Aliaser.GetIdForAlias(alias);
        if(idFromUnderlying != null)
        {
            return CombatantDescriber.NameOf(idFromUnderlying);
        }
        if(AliasFallbackNames.TryGetValue(alias, out var name))
        {
            return name;
        }

        return null;
    }

    public IAliasCombatantIds GetInternalAliaser()
    {
        return Aliaser;
    }

    public DescribeWithAliases WithOverrideWhenNotDefined(CombatantAlias alias, string specificName)
    {
        if (AliasFallbackNames.ContainsKey(alias))
        {// already defined, don't override
            return this;
        }
        var newFallbacks = new Dictionary<CombatantAlias, string>(AliasFallbackNames);
        newFallbacks[alias] = specificName;
        return new DescribeWithAliases(CombatantDescriber, Aliaser, newFallbacks);
    }

    public IDescriptionContext WithAliasOverride(IAliasCombatantIds overrideAlias)
    {
        if (overrideAlias == null) return this;
        var newAliaser = this.Aliaser.OverrideWith(overrideAlias);
        return new DescribeWithAliases(CombatantDescriber, newAliaser, AliasFallbackNames);
    }
}