using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Whenever.Core.WorldInterface
{
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
        
        public static WheneverManager<TInspectWorld, TCommandWorld> Create(TInspectWorld inspector, TCommandWorld commander)
        {
            return new WheneverManager<TInspectWorld, TCommandWorld>(inspector, commander);
        }
        
        public static WheneverManager<TInspectWorld, TCommandWorld> Create<TWorld>(TWorld world)
        where TWorld : TInspectWorld, TCommandWorld
        {
            return new WheneverManager<TInspectWorld, TCommandWorld>(world, world);
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