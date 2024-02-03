using System;
using System.Collections.Generic;
using System.Linq;
using WheneverAbstractions._Project.WheneverAbstractions.Commands;

namespace WheneverAbstractions._Project.WheneverAbstractions
{
    public class GlobalCombatWorld : IInspectableWorld
    {
        private List<Whenever> whenevers = new();
        private Dictionary<CombatantId, Combatant> allCombatants;
        private Random rng;
        public Random GetRng() => rng;
        
        public GlobalCombatWorld(List<Combatant> allCombatants, uint? seed = null)
        {
            seed ??= (uint) DateTime.Now.Ticks;
            rng = new Random((int) seed);
            this.allCombatants = new();
            var id = CombatantId.DEFAULT;
            foreach (var combatant in allCombatants)
            {
                id = CombatantId.Next(id);
                this.allCombatants[id] = combatant;
            }
            
        }

        public IEnumerable<CombatantId> GetOfType(CombatantType type)
        {
            return allCombatants
                .Where(x => x.Value.combatantType == type)
                .Select(x => x.Key);
        }
        
        public IEnumerable<CombatantId> AllIds()
        {
            return allCombatants.Keys;
        }

        public ICombatantData CombatantData(CombatantId combatantId)
        {
            return allCombatants[combatantId];
        }

        private Combatant GetCombatantRaw(CombatantId combatantId)
        {
            return allCombatants[combatantId];
        }
        /// <summary>
        /// applies all status effects currently on the player(s)
        /// </summary>
        public void StartPlayerTurn()
        {
            GenerateAndApplyStatusEffectsFor(CombatantType.Player);
        }

        public void StartEnemyTurn()
        {
            GenerateAndApplyStatusEffectsFor(CombatantType.Enemy);
        }
        
        private void GenerateAndApplyStatusEffectsFor(CombatantType type)
        {
            var playerKvps = allCombatants.Where(x => x.Value.combatantType == type);
            var resultantCommands = new List<InitiatedCommand>();
            foreach (var combatant in playerKvps)
            {
                resultantCommands.AddRange(combatant.Value.ApplyStatusEffects(combatant.Key));
            }
            
            foreach (var command in resultantCommands)
            {
                InitiateCommand(command);
            }
        }

        public void InitiateCommand(IWorldCommand command, ICommandInitiator initiator)
        {
            this.InitiateCommand(new InitiatedCommand(command, initiator));
        }
        public void InitiateCommand(InitiatedCommand command)
        {
            var commandableWorld = new CommandableWorld(this);
            
            var initiatedCommandQueue = new Queue<InitiatedCommand>();
            initiatedCommandQueue.Enqueue(command);
            
            while (initiatedCommandQueue.Count > 0)
            {
                var commandToExecute = initiatedCommandQueue.Dequeue();
                foreach (var whenever in whenevers)
                {
                    foreach (var toQueue in whenever.GetTriggeredCommands(commandToExecute, this))
                    {
                        initiatedCommandQueue.Enqueue(toQueue);
                    }
                }
                
                commandToExecute.command.ApplyCommand(commandableWorld);
            }
        }

        private class CommandableWorld : ICommandableWorld
        {
            private readonly GlobalCombatWorld world;

            public CommandableWorld(GlobalCombatWorld world)
            {
                this.world = world;
            }

            public Combatant GetCombatantRaw(CombatantId combatantId)
            {
                return world.GetCombatantRaw(combatantId);
            }

            public void AddWhenever(Whenever whenever)
            {
                world.whenevers.Add(whenever);
            }
        }
    }

    public interface ICommandableWorld
    {
        Combatant GetCombatantRaw(CombatantId combatantId);
        void AddWhenever(Whenever whenever);
    }

    public interface IInspectableWorld
    {
        public ICombatantData CombatantData(CombatantId combatantId);
        public IEnumerable<CombatantId> AllIds();
        public Random GetRng();
    }
}