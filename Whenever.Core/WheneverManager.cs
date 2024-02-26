using System;
using System.Collections.Generic;
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
    
    
        
    public Guid? AddWhenever(Whenever<TInspectWorld, TCommandWorld> whenever)
    {
        if (whenever == null) return null;
        
        var newWhenever = whenever.ForceRegenerateIdentifier(); 
        whenevers.Add(newWhenever);
        return newWhenever.Id;
    }
    
    public void RemoveWhenever(Guid wheneverId)
    {
        var toRemove = GetWhenever(wheneverId);
        if (toRemove != null)
        {
            whenevers.Remove(toRemove);
        }
    }
    
    public IEnumerable<Guid> GetAllWhenevers()
    {
        return whenevers.Select(w => w.Id);
    }
    
    public Whenever<TInspectWorld, TCommandWorld> GetWhenever(Guid wheneverId)
    {
        return whenevers.FirstOrDefault(w => w.Id == wheneverId);
    }

    public void ClearAll()
    {
        whenevers.Clear();
    }


    public IEnumerable<WheneverExecutionEvent<TInspectWorld, TCommandWorld>> GetAllExecutedEvents(
        IEnumerable<InitiatedCommand<TCommandWorld>> initiatedCommands,
        IAliasCombatantIds aliaser = null
        )
    {
        aliaser ??= new SimpleCombatantAliaser();
        var currentCommandBatch = new List<InitiatedCommand<TCommandWorld>>(initiatedCommands);
        var events = currentCommandBatch.Select(WheneverExecutionEvent<TInspectWorld, TCommandWorld>.FromOriginCommand).ToList();
        var remainingWhenevers = whenevers.ToList();
        while (remainingWhenevers.Count > 0)
        {
            // the first whenever in the list which is triggered by any of the current commands
            var matchedWhenever = remainingWhenevers.FirstOrDefault(whenever => currentCommandBatch
                .Any(c => whenever.TriggersOn(c, aliaser, inspector)));
            // if no whenevers match, we're done
            if (matchedWhenever == null) break;
            remainingWhenevers.Remove(matchedWhenever);
            
            var newCommands = new List<InitiatedCommand<TCommandWorld>>();
            foreach (var initiatedCommand in currentCommandBatch)
            {
                var triggered = matchedWhenever.GetTriggeredCommands(initiatedCommand, aliaser, inspector).ToList();
                if (!triggered.Any()) continue;
                newCommands.AddRange(triggered);
                events.AddRange(triggered.Select(c => 
                    WheneverExecutionEvent<TInspectWorld, TCommandWorld>.FromTriggeredCommand(c, matchedWhenever, initiatedCommand)));
            }
            currentCommandBatch.AddRange(newCommands);
        }
        
        return events;
    }
    
    public void InitiateCommandBatch(
        IEnumerable<InitiatedCommand<TCommandWorld>> initiatedCommands,
        IDescribeCombatants descriptionContext = null,
        IAliasCombatantIds aliaser = null)
    {
        aliaser ??= new SimpleCombatantAliaser();
        descriptionContext ??= SimpleDescriptionContext.CreateInstance();
        
        var allExecutedEvents = GetAllExecutedEvents(initiatedCommands, aliaser);
        
        foreach (var currentCommand in allExecutedEvents)
        {
            Debug.Log("Applying command: " + currentCommand.generatedCommand.Describe(descriptionContext));
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
