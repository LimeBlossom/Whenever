public class TargetIsExactly<TInspectWorld, TCommandWorld>: IWheneverFilter<TInspectWorld, TCommandWorld>
    where TInspectWorld : IInspectWorld
    where TCommandWorld : ICommandWorld
{
    public readonly CombatantId id;

    public TargetIsExactly(CombatantId id)
    {
        this.id = id;
    }
    
    public bool TriggersOn(InitiatedCommand<TCommandWorld> initiatedCommand, TInspectWorld world)
    {
        if (initiatedCommand.command is not IGenericTargetedWorldCommand<TCommandWorld> targetedCommand) return false;
        
        return targetedCommand.Target == id;
    }

    public string Describe(IDescriptionContext context)
    {
        return $"{context.TargetName} is {context.NameOf(id)}";
    }
}