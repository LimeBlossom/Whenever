using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WheneverAbstractions._Project.WheneverAbstractions;
using WheneverAbstractions._Project.WheneverAbstractions.CommandInitiators;
using WheneverAbstractions._Project.WheneverAbstractions.Commands;
using WheneverAbstractions._Project.WheneverAbstractions.PrimitiveUtilities;
using Random = System.Random;

namespace WheneverAbstractions._Project.WheneverAbstractions
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
            
            var initiatedCommandQueue = new Queue<InitiatedCommand>();
            initiatedCommandQueue.Enqueue(command);
            
            var wheneversRemaining = whenevers.ToList();
            while (initiatedCommandQueue.Count > 0)
            {
                var commandToExecute = initiatedCommandQueue.Dequeue();
                if (commandToExecute.initiator is RecursiveEffectCommandInitiator { EffectDepth: > 1000 })
                {
                    throw new Exception("Recursion depth exceeded 1000, likely infinite loop in effect. Aborting.");
                }
                foreach (var whenever in wheneversRemaining.ToList())
                {
                    var triggers = whenever.GetTriggeredCommands(commandToExecute, this).ToList();
                    if (triggers.Any())
                    {
                        wheneversRemaining.Remove(whenever);
                    }
                    foreach (var toQueue in triggers)
                    {
                        initiatedCommandQueue.Enqueue(toQueue);
                    }
                }
                
                Debug.Log("Applying command: " + commandToExecute);
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
        
        public CombatantId GetAtLocation(Vector2 location);
        
    }
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