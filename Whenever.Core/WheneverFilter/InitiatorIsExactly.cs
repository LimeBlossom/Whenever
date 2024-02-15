public class InitiatorIsExactly<TInspectWorld, TCommandWorld>: IWheneverFilter<TInspectWorld, TCommandWorld>
    where TInspectWorld : IInspectWorld
    where TCommandWorld : ICommandWorld
{
    public readonly CombatantId id;

    public InitiatorIsExactly(CombatantId id)
    {
        this.id = id;
    }
    
    public bool TriggersOn(InitiatedCommand<TCommandWorld> initiatedCommand, TInspectWorld world)
    {
        if (!initiatedCommand.initiator.TryAsOrRecursedFrom<CombatantCommandInitiator>(out var initiator))
        {
            return false;
        }

        return initiator.Initiator == id;
    }

    public string Describe(IDescriptionContext context)
    {
        return $"{context.InitiatorName} is {context.NameOf(id)}";
    }
}