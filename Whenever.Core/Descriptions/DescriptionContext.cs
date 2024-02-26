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
    public string InitiatorName { get; }
    public string TargetName { get; }
}

public static class DescriptionContextExtensions
{
    private class TargetOverrideDescriptionContext : IDescriptionContext
    {
        private readonly IDescriptionContext context;
        private readonly string targetName;
        public TargetOverrideDescriptionContext(IDescriptionContext context, string targetName)
        {
            this.context = context;
            this.targetName = targetName;
        }
        public string NameOf(CombatantId id)
        {
            return context.NameOf(id);
        }

        public string InitiatorName => context.InitiatorName;

        public string TargetName => targetName;
        public string NameOf(CombatantAlias alias)
        {
            return context.NameOf(alias);
        }
    }
    
    public static IDescriptionContext WithTargetOverride(this IDescriptionContext context, string targetName)
    {
        return new TargetOverrideDescriptionContext(context, targetName);
    }
    
    private class SpecificOverrideDescriptionContext : IDescriptionContext
    {
        private readonly IDescriptionContext context;
        private readonly CombatantId overrideId;
        private readonly string specificName;
        public SpecificOverrideDescriptionContext(IDescriptionContext context, CombatantId overrideId, string specificName)
        {
            this.context = context;
            this.overrideId = overrideId;
            this.specificName = specificName;
        }
        public string NameOf(CombatantId id)
        {
            return id == overrideId ? specificName : context.NameOf(id);
        }

        public string InitiatorName => context.InitiatorName;

        public string TargetName => context.TargetName;
        public string NameOf(CombatantAlias alias)
        {
            return context.NameOf(alias);
        }
    }
    
    public static IDescriptionContext WithSpecificOverride(this IDescriptionContext context, CombatantId specific, string specificName)
    {
        return new SpecificOverrideDescriptionContext(context, specific, specificName);
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

        public string InitiatorName => underlyingDescription.InitiatorName;

        public string TargetName => underlyingDescription.TargetName;
        public string NameOf(CombatantAlias alias)
        {
            var overrideAliasId = overrideAlias.GetIdForAlias(alias);
            if (overrideAliasId == null) return underlyingDescription.NameOf(alias);
            return underlyingDescription.NameOf(overrideAliasId);
        }
    }
    
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
    
    public static string ToTargetAsDirectSubject(this IDescriptionContext context)
    {
        var targetName = context.TargetName;
        if (string.IsNullOrWhiteSpace(targetName))
        {
            return "";
        }
        else
        {
            return " to " + targetName;
        }
    }
    
    public static string ToInitiatorAsDirectSubject(this IDescriptionContext context)
    {
        var initiatorName = context.InitiatorName;
        if (string.IsNullOrWhiteSpace(initiatorName))
        {
            return "";
        }
        else
        {
            return " to " + initiatorName;
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