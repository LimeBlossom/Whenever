
public interface IDescribeAliases
{
    public string TryNameOf(CombatantAlias alias);
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
        return context.TryNameOf(alias);
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