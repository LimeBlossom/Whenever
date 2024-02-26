
using UnityEngine;

public static class FallbackPriority
{
    public const int CombatantAliasRawId = -1000;
    public const int BaseCombatantAliasNames = -100;
    public const int OverrideBaseAliasNameL1 = -90;
    public const int OverrideBaseAliasNameL2 = -80;
    public const int NamedCombatant = 100;
}

public class DescribeWithAliases: IDescriptionContext
{
    public IDescribeCombatants CombatantDescriber { get; }
    public IAliasCombatantIds Aliaser { get; }
    public IReadonlyFallbackNames<CombatantAlias, string> AliasFallbackNames { get; }

    private DescribeWithAliases(
        IDescribeCombatants combatantDescriber,
        IAliasCombatantIds aliaser,
        IReadonlyFallbackNames<CombatantAlias, string> aliasFallbackNames)
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
        var fallbackNames = new FallbackNames<CombatantAlias, string>();
        fallbackNames.AddFallback(StandardAliases.Initiator, initiatorName, FallbackPriority.BaseCombatantAliasNames);
        fallbackNames.AddFallback(StandardAliases.Target, targetName, FallbackPriority.BaseCombatantAliasNames);
        return new DescribeWithAliases(combatantDescriber, aliaser, fallbackNames);
    }
    
    public static DescribeWithAliases WithDefinedAliasNames(
        IDescribeCombatants combatantDescriber,
        IAliasCombatantIds aliaser,
        IReadonlyFallbackNames<CombatantAlias, string> aliasFallbackNames)
    {
        return new DescribeWithAliases(combatantDescriber, aliaser, aliasFallbackNames);
    }

    public string NameOf(CombatantId id)
    {
        return CombatantDescriber.NameOf(id);
    }

    public string NameOf(CombatantAlias alias)
    {
        var immediatePriority = FallbackPriority.CombatantAliasRawId;
        var immediateFallback = FallbackPriority.CombatantAliasRawId.ToString();
        
        var idFromUnderlying = Aliaser.GetIdForAlias(alias);
        if(idFromUnderlying != null)
        {
            immediatePriority = FallbackPriority.NamedCombatant;
            immediateFallback = CombatantDescriber.NameOf(idFromUnderlying);
        }
        
        var finalName = AliasFallbackNames.GetFallbackValue(alias, immediateFallback, immediatePriority);
        if (finalName.priority <= FallbackPriority.CombatantAliasRawId)
        {
            Debug.LogWarning($"No name defined for alias {alias}");
        }
        return finalName.val;
    }

    public DescribeWithAliases WithOverrideWhenNotDefined(CombatantAlias alias, string specificName, int priority)
    {
        var existingFallback = AliasFallbackNames.GetFallbackWithPriority(alias);
        if (existingFallback is not null && existingFallback.Value.priority > priority)
        {// already defined with higher priority, don't override
            return this;
        }
        var newFallbacks = new FallbackNames<CombatantAlias, string>(AliasFallbackNames);
        newFallbacks.AddFallback(alias, specificName, priority);
        return new DescribeWithAliases(CombatantDescriber, Aliaser, newFallbacks);
    }

    public IDescriptionContext WithAliasOverride(IAliasCombatantIds overrideAlias)
    {
        if (overrideAlias == null) return this;
        var newAliaser = this.Aliaser.OverrideWith(overrideAlias);
        return new DescribeWithAliases(CombatantDescriber, newAliaser, AliasFallbackNames);
    }
}