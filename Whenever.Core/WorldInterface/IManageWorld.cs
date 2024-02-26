using System;
using System.Collections.Generic;
using UnityEngine;

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
    public void InitiateCommandBatch(
        IEnumerable<InitiatedCommand<TCommand>> initiatedCommand,
        IAliasCombatantIds aliaser = null);
    public IEnumerable<WheneverExecutionEvent<TInspect, TCommand> > GetAllExecutedEvents(
        IEnumerable<InitiatedCommand<TCommand>> initiatedCommand,
        IAliasCombatantIds aliaser = null);
        
    public Guid? AddWhenever(Whenever<TInspect, TCommand> whenever);
    public void RemoveWhenever(Guid wheneverId);
    public Whenever<TInspect, TCommand> GetWhenever(Guid wheneverId);

    public IEnumerable<Guid> GetAllWhenevers();
}
    
public static class ManageWorldExtensions
{
    public static void InitiateCommand<TInspect, TCommand>(
        this IManageWorld<TInspect, TCommand> manager,
        IWorldCommand<TCommand> command,
        ICommandInitiator initiator,
        IAliasCombatantIds aliaser = null)
        where TInspect : IInspectWorld
        where TCommand : ICommandWorld
    {
        manager.InitiateCommand(new InitiatedCommand<TCommand>(command, initiator), aliaser);
    }
        
    public static void InitiateCommand<TInspect, TCommand>(
        this IManageWorld<TInspect, TCommand> manager,
        InitiatedCommand<TCommand> command,
        IAliasCombatantIds aliaser = null)
        where TInspect : IInspectWorld
        where TCommand : ICommandWorld
    {
        manager.InitiateCommandBatch(new []{command}, aliaser);
    }
    
    public static void RemoveWhenever<TInspect, TCommand>(
        this IManageWorld<TInspect, TCommand> manager, 
        Whenever<TInspect, TCommand> whenever)
        where TInspect : IInspectWorld
        where TCommand : ICommandWorld
    {
        var toRemove = manager.GetWhenever(whenever.Id);
        if (toRemove != null)
        {
            if(toRemove != whenever)
            {
                Debug.LogWarning("WheneverManager: requested to remove a whenever, but found whenever is not exact match for removed whenever.");
            }
            manager.RemoveWhenever(whenever.Id);
        }
    }
        
}