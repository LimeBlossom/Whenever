using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Whenever.Core.WorldInterface
{
    public class CoreActorWorld<TCombatant> :
        IInspectWorld,
        ICommandWorld,
        IManageWorld<IInspectWorld, ICommandWorld>
    {
        private List<Whenever<IInspectWorld, ICommandWorld>> whenevers = new();
        protected Dictionary<CombatantId, TCombatant> allCombatants;

        public CoreActorWorld(List<TCombatant> allCombatants)
        {
            this.allCombatants = new();
            var id = CombatantId.DEFAULT;
            foreach (var combatant in allCombatants)
            {
                id = CombatantId.Next(id);
                this.allCombatants[id] = combatant;
            }
            
        }

        public IEnumerable<CombatantId> AllIds()
        {
            return allCombatants.Keys;
        }

        public TCombatant InspectCombatant(CombatantId combatantId)
        {
            return allCombatants[combatantId];
        }

        public TCombatant CommandCombatant(CombatantId combatantId)
        {
            return allCombatants[combatantId];
        }
        
        public void AddWhenever(Whenever<IInspectWorld, ICommandWorld> whenever)
        {
            whenevers.Add(whenever);
        }

        public void InitiateCommandBatch(IEnumerable<InitiatedCommand<ICommandWorld>> initiatedCommands)
        {
            var commandableWorld = this;

            var currentCommandBatch = new List<InitiatedCommand<ICommandWorld>>(initiatedCommands);

            foreach (var whenever in whenevers)
            {
                var newCommands = new List<InitiatedCommand<ICommandWorld>>();
                foreach (var initiatedCommand in currentCommandBatch)
                {
                    var triggered = whenever.GetTriggeredCommands(initiatedCommand, this).ToList();
                    if (!triggered.Any()) continue;
                    newCommands.AddRange(triggered);
                }
                currentCommandBatch.AddRange(newCommands);
            }
                
            foreach (var currentCommand in currentCommandBatch)
            {
                Debug.Log("Applying command: " + currentCommand);
                currentCommand.command.ApplyCommand(commandableWorld);
            }
        }
        
        public void SaySomething(CombatantId id, string message)
        {
            throw new System.NotImplementedException();
        }
    }
}