public interface IDescribableWithContext
{
    public string Describe(IDescriptionContext context);
}

public interface IDescribableWithConcreteContext
{
    public string Describe(DescribeWithAliases contextWithAliases);
}

public static class DescribableWithContextExtensions
{
    public static string DescribeSentenceCase(
        this IDescribableWithContext describable,
        IDescriptionContext context)
    {
        var description = describable.Describe(context);
        return UppercaseFirst(description);
    }
    public static string DescribeSentenceCase(
        this IDescribableWithConcreteContext describable,
        DescribeWithAliases context)
    {
        var description = describable.Describe(context);
        return UppercaseFirst(description);
    }
    
    private static string UppercaseFirst(string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return string.Empty;
        }
        return char.ToUpper(s[0]) + s.Substring(1);
    }
}