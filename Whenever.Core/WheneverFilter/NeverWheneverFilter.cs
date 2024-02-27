public class NeverWheneverFilter<TInspectWorld, TCommandWorld>: IWheneverFilter<TInspectWorld, TCommandWorld>
    where TInspectWorld : IInspectWorld
    where TCommandWorld : ICommandWorld
{
    private readonly string description;

    public NeverWheneverFilter(string description = "never")
    {
        this.description = description;
    }


    public bool TriggersOn(
        InitiatedCommand<TCommandWorld> initiatedCommand,
        IAliasCombatantIds aliaser,
        TInspectWorld world)
    {
        return false;
    }

    public string Describe(IDescriptionContext context)
    {
        return description;
    }
}