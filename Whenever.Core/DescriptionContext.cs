public interface IDescriptionContext
{
    public string InitiatorName { get; }
    public string TargetName { get; }
}

public class SimpleDescriptionContext : IDescriptionContext
{
    public SimpleDescriptionContext(): this("the initiator", "the target")
    {
    }
    public SimpleDescriptionContext(string initiatorName, string targetName)
    {
        this.InitiatorName = initiatorName;
        this.TargetName = targetName;
    }

    public string InitiatorName { get; set; }
    public string TargetName { get; set;  }
}