
using UnityEngine;
using static FallbackConstants;

public static class FallbackConstants
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
        fallbackNames.AddFallback(StandardAliases.Initiator, initiatorName, BaseCombatantAliasNames);
        fallbackNames.AddFallback(StandardAliases.Target, targetName, BaseCombatantAliasNames);
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

    public string TryNameOf(CombatantAlias alias)
    {
        var immediatePriority = CombatantAliasRawId;
        var immediateFallback = CombatantAliasRawId.ToString();
        
        var idFromUnderlying = Aliaser.GetIdForAlias(alias);
        if(idFromUnderlying != null)
        {
            immediatePriority = NamedCombatant;
            immediateFallback = CombatantDescriber.NameOf(idFromUnderlying);
        }
        
        var finalName = AliasFallbackNames.GetFallbackValue(alias, immediateFallback, immediatePriority);
        if (finalName.priority <= CombatantAliasRawId)
        {
            Debug.LogWarning($"No name defined for alias {alias}");
        }
        return finalName.val;
    }

    public IAliasCombatantIds GetInternalAliaser()
    {
        return Aliaser;
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