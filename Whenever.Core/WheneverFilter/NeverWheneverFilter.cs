public class NeverWheneverFilter<TInspectWorld, TCommandWorld>: IWheneverFilter<TInspectWorld, TCommandWorld>
    where TInspectWorld : IInspectWorld
    where TCommandWorld : ICommandWorld
{
    public bool TriggersOn(InitiatedCommand<TCommandWorld> initiatedCommand, TInspectWorld world)
    {
        return false;
    }

    public string Describe(IDescriptionContext context)
    {
        return "never";
    }
}