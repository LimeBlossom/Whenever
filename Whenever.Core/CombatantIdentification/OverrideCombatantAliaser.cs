using System.Collections.Generic;
using System.Linq;

public class OverrideCombatantAliaser : IAliasCombatantIds
{
    private readonly IAliasCombatantIds underlyingAliaser;
    private readonly IAliasCombatantIds overrideAliaser;

    internal OverrideCombatantAliaser(IAliasCombatantIds underlyingAliaser, IAliasCombatantIds overrideAliaser)
    {
        this.underlyingAliaser = underlyingAliaser;
        this.overrideAliaser = overrideAliaser;
    }
    
    public CombatantId GetIdForAlias(CombatantAlias alias)
    {
        return overrideAliaser.GetIdForAlias(alias) ?? underlyingAliaser.GetIdForAlias(alias);
    }

    public IEnumerable<CombatantAlias> AllDefinedAliases()
    {
        return underlyingAliaser.AllDefinedAliases().Concat(overrideAliaser.AllDefinedAliases()).Distinct();
    }
}

public static class OverrideAliaserExtensions
{
    public static IAliasCombatantIds WithOverrides(this IAliasCombatantIds aliaser, params (CombatantAlias, CombatantId)[] overrides)
    {
        var overrideAliaser = new SimpleCombatantAliaser(overrides);
        return aliaser.OverrideWith(overrideAliaser);
    }
    public static IAliasCombatantIds OverrideWith(this IAliasCombatantIds aliaser, IAliasCombatantIds overrides)
    {
        return overrides == null ?
            aliaser :
            new OverrideCombatantAliaser(aliaser, overrides);
    }
}