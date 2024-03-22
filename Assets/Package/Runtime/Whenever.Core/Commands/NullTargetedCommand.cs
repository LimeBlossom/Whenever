public class NullTargetedCommand<TCommand> : IGenericTargetedWorldCommand<TCommand>
    where TCommand: ICommandWorld
{
    public CombatantId Target { get; set;  }
    public void ApplyCommand(TCommand world)
    {
        // noop
    }

    public string Describe(IDescribeCombatants context)
    {
        return "do nothing to " + context.NameOf(Target);
    }
}