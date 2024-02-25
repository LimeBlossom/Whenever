using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

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
}

public static class OverrideAliaserExtensions
{
    public static IAliasCombatantIds WithOverrides(this IAliasCombatantIds aliaser, params (CombatantAlias, CombatantId)[] overrides)
    {
        var overrideAliaser = new SimpleCombatantAliaser();
        foreach (var (alias, id) in overrides)
        {
            overrideAliaser.SetAlias(alias, id);
        }
        return new OverrideCombatantAliaser(aliaser, overrideAliaser);
    }
    public static IAliasCombatantIds OverrideWith(this IAliasCombatantIds aliaser, IAliasCombatantIds overrides)
    {
        return overrides == null ?
            aliaser :
            new OverrideCombatantAliaser(aliaser, overrides);
    }
}