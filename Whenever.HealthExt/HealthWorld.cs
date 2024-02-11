using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Whenever.Core.CommandInitiators;
using Whenever.Core.StatusEffects;
using Whenever.Core.WorldInterface;

namespace Whenever.HealthExt
{
    public class HealthWorld : IInspectWorldHealth, ICommandWorldHealth, IManageWorld<IInspectWorldHealth, ICommandWorldHealth>
    {

        private List<Whenever.Core.Whenever<IInspectWorldHealth, ICommandWorldHealth>> whenevers = new();
        protected Dictionary<CombatantId, HealthCombatant> allCombatants;

        public HealthWorld(List<HealthCombatant> allCombatants)
        {
            this.allCombatants = new();
            var id = CombatantId.DEFAULT;
            foreach (var combatant in allCombatants)
            {
                id = CombatantId.Next(id);
                combatant.id = id;
                this.allCombatants[id] = combatant;
            }
        }

        public float GetHealth(CombatantId id)
        {
            return InspectCombatant(id).health;
        }

        public void DoDamage(CombatantId id, float health)
        {
            var combatant = InspectCombatant(id);
            combatant.health -= health;
        }

        public void AddStatusEffect(CombatantId id, StatusEffect effect)
        {
            var combatant = InspectCombatant(id);
            combatant.statusEffects.Add(effect);
        }
        public IEnumerable<CombatantId> AllIds()
        {
            return allCombatants.Keys;
        }

        public HealthCombatant InspectCombatant(CombatantId combatantId)
        {
            return allCombatants[combatantId];
        }

        public void ApplyAllStatusEffects()
        {
            var resultantCommands = new List<InitiatedCommand<ICommandWorldHealth>>();
            foreach (var combatant in allCombatants)
            {
                resultantCommands.AddRange(combatant.Value.ApplyStatusEffects(combatant.Key));
            }
        
            foreach (var command in resultantCommands)
            {
                InitiateCommandBatch(new List<InitiatedCommand<ICommandWorldHealth>>{command});
            }
        }

        public HealthCombatant CommandCombatant(CombatantId combatantId)
        {
            return allCombatants[combatantId];
        }
        
        public void AddWhenever(Whenever.Core.Whenever<IInspectWorldHealth, ICommandWorldHealth> whenever)
        {
            whenevers.Add(whenever);
        }

        public void InitiateCommandBatch(IEnumerable<InitiatedCommand<ICommandWorldHealth>> initiatedCommands)
        {
            var commandableWorld = this;

            var currentCommandBatch = new List<InitiatedCommand<ICommandWorldHealth>>(initiatedCommands);

            foreach (var whenever in whenevers)
            {
                var newCommands = new List<InitiatedCommand<ICommandWorldHealth>>();
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

    public class HealthCombatant
    {
        public float health;
        public List<StatusEffect> statusEffects = new();
        [CanBeNull] public CombatantId id; 
        
        public HealthCombatant(float health)
        {
            this.health = health;
        }
        public IEnumerable<InitiatedCommand<ICommandWorldHealth>> ApplyStatusEffects(CombatantId myId)
        {
            foreach(StatusEffect statusEffect in statusEffects.ToArray())
            {
                var statusEffectResult = statusEffect.ActivateOn(myId);
                if (statusEffectResult.completion == StatusEffectCompletion.Expired)
                {
                    statusEffects.Remove(statusEffect);
                }
                foreach (var command in statusEffectResult.commands)
                {
                    yield return new InitiatedCommand<ICommandWorldHealth>(command, Initiators.Factory.From(statusEffect));
                }
            }
        }

    }
}