public interface IWheneverFilter<in TInspectWorld, TCommandWorld> : IDescribableWithContext
    where TInspectWorld : IInspectWorld
    where TCommandWorld : ICommandWorld
{
    public bool TriggersOn(
        InitiatedCommand<TCommandWorld> initiatedCommand,
        IAliasCombatantIds aliaser,
        TInspectWorld world);
}