﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// holds on to a list of whenevers, and can execute a command batch, applying all whenevers to the batch of commands.
/// Also holds on to an inspectable and commandable world, which will be used to evaluate whenever conditions, or apply commands.
/// </summary>
/// <typeparam name="TInspectWorld"></typeparam>
/// <typeparam name="TCommandWorld"></typeparam>
public class WheneverManager<TInspectWorld, TCommandWorld> : IManageWorld<TInspectWorld, TCommandWorld>
    where TInspectWorld : IInspectWorld
    where TCommandWorld : ICommandWorld
{
    private readonly TInspectWorld inspector;
    private readonly TCommandWorld commander;
    private List<Whenever<TInspectWorld, TCommandWorld>> whenevers = new();

    public WheneverManager(TInspectWorld inspector, TCommandWorld commander)
    {
        this.inspector = inspector;
        this.commander = commander;
    }
        
    public void AddWhenever(Whenever<TInspectWorld, TCommandWorld> whenever)
    {
        if(whenever != null)
        {
            whenevers.Add(whenever);
        }
    }

    public void Clear()
    {
        whenevers.Clear();
    }


    public IEnumerable<WheneverExecutionEvent<TInspectWorld, TCommandWorld>> GetAllExecutedEvents(IEnumerable<InitiatedCommand<TCommandWorld>> initiatedCommands)
    {
        var currentCommandBatch = new List<InitiatedCommand<TCommandWorld>>(initiatedCommands);
        var events = currentCommandBatch.Select(WheneverExecutionEvent<TInspectWorld, TCommandWorld>.FromOriginCommand).ToList();
        foreach (var whenever in whenevers)
        {
            var newCommands = new List<InitiatedCommand<TCommandWorld>>();
            foreach (var initiatedCommand in currentCommandBatch)
            {
                var triggered = whenever.GetTriggeredCommands(initiatedCommand, inspector).ToList();
                if (!triggered.Any()) continue;
                newCommands.AddRange(triggered);
                events.AddRange(triggered.Select(c => WheneverExecutionEvent<TInspectWorld, TCommandWorld>.FromTriggeredCommand(c, whenever, initiatedCommand)));
            }
            currentCommandBatch.AddRange(newCommands);
        }
        return events;
    }
    
    public void InitiateCommandBatch(IEnumerable<InitiatedCommand<TCommandWorld>> initiatedCommands)
    {
        var allExecutedEvents = GetAllExecutedEvents(initiatedCommands);
        foreach (var currentCommand in allExecutedEvents)
        {
            Debug.Log("Applying command: " + currentCommand.generatedCommand.Describe());
            currentCommand.generatedCommand.command.ApplyCommand(commander);
        }
    }
}

public record WheneverExecutionEvent<TInspect, TCommand> 
    where TInspect : IInspectWorld
    where TCommand : ICommandWorld
{
    public InitiatedCommand<TCommand> triggeringCommand { get; private set; }
    public Whenever<TInspect, TCommand> triggeredWhenever { get; private set; }
    public InitiatedCommand<TCommand> generatedCommand { get; private set; }
        
    private WheneverExecutionEvent()
    {
    }
        
    public static WheneverExecutionEvent<TInspect, TCommand>  FromOriginCommand(InitiatedCommand<TCommand> command)
    {
        return new WheneverExecutionEvent<TInspect, TCommand> 
        {
            triggeringCommand = null,
            triggeredWhenever = null,
            generatedCommand = command,
        };
    }
        
    public static WheneverExecutionEvent<TInspect, TCommand>  FromTriggeredCommand(
        InitiatedCommand<TCommand> command,
        Whenever<TInspect, TCommand> source, 
        InitiatedCommand<TCommand> triggeredBy)
    {
        return new WheneverExecutionEvent<TInspect, TCommand> 
        {
            triggeringCommand = triggeredBy,
            triggeredWhenever = source,
            generatedCommand = command,
        };
    }
}
