using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Whenever.Core.CommandInitiators;
using Whenever.Core.WorldInterface;
using Random = System.Random;

namespace Whenever.Core.WheneverTestDemo
{
    public class GlobalCombatWorldDemo : IInspectableWorldDemo, ICommandableWorldDemo, IManageWorld<IInspectableWorldDemo, ICommandableWorldDemo>
    {
        private List<Whenever<IInspectableWorldDemo, ICommandableWorldDemo>> whenevers = new();
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

        public GlobalCombatWorldDemo(List<Combatant> allCombatants, uint? seed = null)
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

        public Combatant GetCombatantRaw(CombatantId combatantId)
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
            var resultantCommands = new List<InitiatedCommand<ICommandableWorldDemo>>();
            foreach (var combatant in playerKvps)
            {
                resultantCommands.AddRange(combatant.Value.ApplyStatusEffects(combatant.Key));
            }
            
            foreach (var command in resultantCommands)
            {
                InitiateCommand(command);
            }
        }

        public void InitiateCommand(IWorldCommand<ICommandableWorldDemo> commandOld, ICommandInitiator initiator)
        {
            this.InitiateCommand(new InitiatedCommand<ICommandableWorldDemo>(commandOld, initiator));
        }

        public void InitiateCommand(InitiatedCommand<ICommandableWorldDemo> command)
        {
            var batch = new List<InitiatedCommand<ICommandableWorldDemo>>(){command};
            InitiateCommandBatch(batch);
        }

            public void SaySomething(CombatantId id, string message)
            {
                throw new NotImplementedException();
            }

            public void InitiateCommandBatch(IEnumerable<InitiatedCommand<ICommandableWorldDemo>> initiatedCommands)
            {
                var commandableWorld = this;

                var currentCommandBatch = new List<InitiatedCommand<ICommandableWorldDemo>>(initiatedCommands);

                foreach (var whenever in whenevers)
                {
                    var newCommands = new List<InitiatedCommand<ICommandableWorldDemo>>();
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

            public void AddWhenever(Whenever<IInspectableWorldDemo, ICommandableWorldDemo> whenever)
            {
                this.whenevers.Add(whenever);
            }
    }

}