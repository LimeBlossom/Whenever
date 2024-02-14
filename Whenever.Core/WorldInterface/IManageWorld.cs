using System.Collections.Generic;

/// <summary>
/// Used to manage the world from the main game loop. The only way to trigger changes externally.
/// </summary>
public interface IManageWorld<TInspect, TCommand>
    where TInspect : IInspectWorld
    where TCommand : ICommandWorld
{
    /// <summary>
    /// Initiate a command chain. Whenevers will apply only once, generating an entire new batch of commands from the
    /// previous command batch.
    /// </summary>
    /// <param name="initiatedCommand"></param>
    /// <param name="descriptionContext"></param>
    public void InitiateCommandBatch(IEnumerable<InitiatedCommand<TCommand>> initiatedCommand, IDescribeCombatants descriptionContext = null);
    public IEnumerable<WheneverExecutionEvent<TInspect, TCommand> > GetAllExecutedEvents(IEnumerable<InitiatedCommand<TCommand>> initiatedCommand);
        
    public void AddWhenever(Whenever<TInspect, TCommand> whenever);
}
    
public static class ManageWorldExtensions
{
    public static void InitiateCommand<TInspect, TCommand>(this IManageWorld<TInspect, TCommand> manager, IWorldCommand<TCommand> command, ICommandInitiator initiator, IDescribeCombatants descriptionContext = null)
        where TInspect : IInspectWorld
        where TCommand : ICommandWorld
    {
        manager.InitiateCommand(new InitiatedCommand<TCommand>(command, initiator), descriptionContext);
    }
        
    public static void InitiateCommand<TInspect, TCommand>(this IManageWorld<TInspect, TCommand> manager, InitiatedCommand<TCommand> command, IDescribeCombatants descriptionContext = null)
        where TInspect : IInspectWorld
        where TCommand : ICommandWorld
    {
        manager.InitiateCommandBatch(new []{command}, descriptionContext);
    }
        
}