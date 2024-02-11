using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Whenever.Core.WorldInterface;

namespace Whenever.Core
{
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
            whenevers.Add(whenever);
        }
        
        public void InitiateCommandBatch(IEnumerable<InitiatedCommand<TCommandWorld>> initiatedCommands)
        {
            var currentCommandBatch = new List<InitiatedCommand<TCommandWorld>>(initiatedCommands);

            foreach (var whenever in whenevers)
            {
                var newCommands = new List<InitiatedCommand<TCommandWorld>>();
                foreach (var initiatedCommand in currentCommandBatch)
                {
                    var triggered = whenever.GetTriggeredCommands(initiatedCommand, inspector).ToList();
                    if (!triggered.Any()) continue;
                    newCommands.AddRange(triggered);
                }
                currentCommandBatch.AddRange(newCommands);
            }
                
            foreach (var currentCommand in currentCommandBatch)
            {
                Debug.Log("Applying command: " + currentCommand);
                currentCommand.command.ApplyCommand(commander);
            }
        }
    }
}