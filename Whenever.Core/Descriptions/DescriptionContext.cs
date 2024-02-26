public interface IDescribeAliases
{
    public string TryNameOf(CombatantAlias alias);
    public IAliasCombatantIds GetInternalAliaser();
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
    public static string NameOf(this IDescribeAliases context, CombatantAlias alias)
    {
        return context.TryNameOf(alias) ?? alias.ReadableDescription;
    }
    
    private class OverrideAliasWhenNotDefined : IDescriptionContext
    {
        private readonly IDescriptionContext context;
        private readonly CombatantAlias aliasOverride;
        private readonly string specificName;
        public OverrideAliasWhenNotDefined(IDescriptionContext context, CombatantAlias aliasOverride, string specificName)
        {
            this.context = context;
            this.aliasOverride = aliasOverride;
            this.specificName = specificName;
        }
        public string NameOf(CombatantId id)
        {
            return context.NameOf(id);
        }

        public string TryNameOf(CombatantAlias alias)
        {
            var underlyingName = context.TryNameOf(alias);
            if (!aliasOverride.Equals(alias)) return underlyingName;
            return underlyingName ?? specificName;
        }

        public IAliasCombatantIds GetInternalAliaser()
        {
            return context.GetInternalAliaser();
        }
    }
    
    /// <summary>
    /// Whenever describing <paramref name="alias"/>, instead return <paramref name="specificName"/>,
    /// only if the description context cannot identify the aliased combatant
    /// </summary>
    /// <param name="context"></param>
    /// <param name="alias"></param>
    /// <param name="specificName"></param>
    /// <returns></returns>
    public static IDescriptionContext WithOverrideWhenNotDefined(this IDescriptionContext context, CombatantAlias alias, string specificName)
    {
        return new OverrideAliasWhenNotDefined(context, alias, specificName);
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

        public string TryNameOf(CombatantAlias alias)
        {
            var overrideAliasId = overrideAlias.GetIdForAlias(alias);
            if (overrideAliasId == null) return underlyingDescription.TryNameOf(alias);
            return underlyingDescription.NameOf(overrideAliasId);
        }

        public IAliasCombatantIds GetInternalAliaser()
        {
            return underlyingDescription.GetInternalAliaser();
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