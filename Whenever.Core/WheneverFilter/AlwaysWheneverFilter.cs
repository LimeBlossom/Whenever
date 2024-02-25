public class AlwaysWheneverFilter<TInspectWorld, TCommandWorld>: IWheneverFilter<TInspectWorld, TCommandWorld>
    where TInspectWorld : IInspectWorld
    where TCommandWorld : ICommandWorld
{
    public bool TriggersOn(
        InitiatedCommand<TCommandWorld> initiatedCommand,
        IAliasCombatantIds aliaser,
        TInspectWorld world)
    {
        return true;
    }

    public string Describe(IDescriptionContext context)
    {
        return "always";
    }
}