using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Whenever.Core.CommandInitiators;
using Whenever.Core.Commands;
using Whenever.Core.PrimitiveUtilities;
using Random = System.Random;

namespace Whenever.Core
{
    public class GlobalCombatWorld : IInspectableWorld
    {
        private List<Whenever> whenevers = new();
        private Dictionary<CombatantId, Combatant> allCombatants;
        private Random rng;
        public Random GetRng() => rng;
        public CombatantId GetAtLocation(Vector2 location)
        {
            return allCombatants
                .Where(x => x.Value.position == location)
                .Select(x => x.Key)
                .SingleOrDefault();
        }

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
            Debug.Log("Starting player turn");
            GenerateAndApplyStatusEffectsFor(CombatantType.Player);
        }

        public void StartEnemyTurn()
        {
            Debug.Log("Starting enemy turn");
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

            var currentCommandBatch = new List<InitiatedCommand>(){command};

            foreach (var whenever in whenevers)
            {
                var newCommands = new List<InitiatedCommand>();
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
        
        public CombatantId GetAtLocation(Vector2 location);
        
    }

    public static class InspectableWorldExtensions{

        public static IEnumerable<CombatantId> GetAdjacentCombatants(this IInspectableWorld world, CombatantId combatantId)
        {
            var combatantData = world.CombatantData(combatantId);
            var adjacentTiles = VectorExtensions.GetAdjacentTiles(combatantData.GetPosition());
            return adjacentTiles
                .Select(world.GetAtLocation)
                .Where(x => x != CombatantId.INVALID);
        }
    }
}