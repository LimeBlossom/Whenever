using System;
using System.Collections.Generic;
using System.Linq;
using WheneverAbstractions._Project.WheneverAbstractions.Commands;

namespace WheneverAbstractions._Project.WheneverAbstractions
{
    public class GlobalCombatWorld
    {
        private List<Whenever> whenevers = new();
        private Dictionary<CombatantId, Combatant> allCombatants;
        
        public GlobalCombatWorld(List<Combatant> allCombatants)
        {
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
            //TODO: filter this through the whenever stack?
            var commandableWorld = new CommandableWorld(this);
            command.command.ApplyCommand(commandableWorld);
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
}