
public static class ContextAwareAliaserExtensions
{
    public static IAliasCombatantIds OverrideWithCommandContext<TCommand>(this IAliasCombatantIds aliaser, InitiatedCommand<TCommand> command)
        where TCommand: ICommandWorld
    {
        command.initiator.TryAsOrRecursedFrom<CombatantCommandInitiator>(out var initiator);
        var target = (command.command as IGenericTargetedWorldCommand<TCommand>)?.Target;

        var commandAwareAliaser = new SimpleCombatantAliaser(
            (StandardAliases.Target, target),
            (StandardAliases.Initiator, initiator?.Initiator)
        );
        return aliaser.OverrideWith(commandAwareAliaser);
    }
}