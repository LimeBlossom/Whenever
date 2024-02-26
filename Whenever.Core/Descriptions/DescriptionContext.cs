public interface IDescribeAliases
{
    public string NameOf(CombatantAlias alias);
}

public interface IDescribeCombatants
{
    public string NameOf(CombatantId id);
}

public interface IDescriptionContext : IDescribeCombatants, IDescribeAliases
{
}

public static class DescriptionContextExtensions
{
    private class SpecificOverrideDescriptionContext : IDescriptionContext
    {
        private readonly IDescriptionContext context;
        private readonly CombatantAlias aliasOverride;
        private readonly string specificName;
        public SpecificOverrideDescriptionContext(IDescriptionContext context, CombatantAlias aliasOverride, string specificName)
        {
            this.context = context;
            this.aliasOverride = aliasOverride;
            this.specificName = specificName;
        }
        public string NameOf(CombatantId id)
        {
            return context.NameOf(id);
        }

        public string NameOf(CombatantAlias alias)
        {
            return alias.Equals(aliasOverride) ? specificName : context.NameOf(alias);
        }
    }
    
    /// <summary>
    /// Whenever describing <paramref name="alias"/>, instead return <paramref name="specificName"/>
    /// </summary>
    /// <param name="context"></param>
    /// <param name="alias"></param>
    /// <param name="specificName"></param>
    /// <returns></returns>
    public static IDescriptionContext WithSpecificOverride(this IDescriptionContext context, CombatantAlias alias, string specificName)
    {
        return new SpecificOverrideDescriptionContext(context, alias, specificName);
    }
    
    private class AliasOverrideDescriptionContext : IDescriptionContext
    {
        private readonly IDescriptionContext underlyingDescription;
        private readonly IAliasCombatantIds overrideAlias;
        public AliasOverrideDescriptionContext(IDescriptionContext underlyingDescription, IAliasCombatantIds overrideAlias)
        {
            this.underlyingDescription = underlyingDescription;
            this.overrideAlias = overrideAlias;
        }
        public string NameOf(CombatantId id)
        {
            return underlyingDescription.NameOf(id);
        }

        public string NameOf(CombatantAlias alias)
        {
            var overrideAliasId = overrideAlias.GetIdForAlias(alias);
            if (overrideAliasId == null) return underlyingDescription.NameOf(alias);
            return underlyingDescription.NameOf(overrideAliasId);
        }
    }
    
    /// <summary>
    /// when describing an aliased combatant, take the id from the override aliaser if available, otherwise passthrough to the underlying context 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="overrideAlias"></param>
    /// <returns></returns>
    public static IDescriptionContext WithAliasOverride(this IDescriptionContext context, IAliasCombatantIds overrideAlias)
    {
        return overrideAlias == null ? context : new AliasOverrideDescriptionContext(context, overrideAlias);
    }
    
    public static string ToAliasAsDirectSubject(this IDescriptionContext context, CombatantAlias alias)
    {
        var name = context.NameOf(alias);
        if (string.IsNullOrWhiteSpace(name))
        {
            return "";
        }
        else
        {
            return " to " + name;
        }
    }
    
    public static string ToSpecificAsDirectSubject(this IDescriptionContext context, CombatantId specific)
    {
        var name = context.NameOf(specific);
        if (string.IsNullOrWhiteSpace(name))
        {
            return "";
        }
        else
        {
            return " to " + name;
        }
    }
}