using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Whenever.Core.WorldInterface
{

    public static class WheneverManagerFactory
    {
        public static WheneverManager<TInspectWorld, TCommandWorld> Create<TInspectWorld, TCommandWorld>(TInspectWorld inspector, TCommandWorld commander)
            where TInspectWorld : IInspectWorld
            where TCommandWorld : ICommandWorld
        {
            return new WheneverManager<TInspectWorld, TCommandWorld>(inspector, commander);
        }
        
        public static WheneverManager<TWorld, TWorld> Create<TWorld>(TWorld world)
            where TWorld: IInspectWorld, ICommandWorld
        {
            return new WheneverManager<TWorld, TWorld>(world, world);
        }
    }
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
    
    public class CoreActorWorld<TCombatant> :
        IInspectWorld,
        ICommandWorld
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

        public void SaySomething(CombatantId id, string message)
        {
            throw new System.NotImplementedException();
        }
    }
}