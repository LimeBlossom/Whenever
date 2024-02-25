using System.Collections.Generic;
using UnityEngine;

public class CommandContextAwareCombatantAliaser<TCommand>  : IAliasCombatantIds
    where TCommand: ICommandWorld
{
    private readonly InitiatedCommand<TCommand> command;

    internal CommandContextAwareCombatantAliaser(InitiatedCommand<TCommand> command)
    {
        this.command = command;
    }
    
    public CombatantId GetIdForAlias(CombatantAlias alias)
    {
        if (alias.Equals(StandardAliases.Target))
        {
            return (command.command as IGenericTargetedWorldCommand<TCommand>)?.Target;
        }
        if(alias.Equals(StandardAliases.Initiator))
        {
            command.initiator.TryAsOrRecursedFrom<CombatantCommandInitiator>(out var initiator);
            return initiator?.Initiator;
        }
        return null;
    }
}

public static class ContextAwareAliaserExtensions
{
    public static IAliasCombatantIds OverrideWithCommandContext<TCommand>(this IAliasCombatantIds aliaser, InitiatedCommand<TCommand> command)
        where TCommand: ICommandWorld
    {
        var commandAwareAliaser = new CommandContextAwareCombatantAliaser<TCommand>(command);
        return new OverrideCombatantAliaser(aliaser, commandAwareAliaser);
    }
}