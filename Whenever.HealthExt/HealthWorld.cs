using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Whenever.Core;
using Whenever.Core.Commands;
using Whenever.Core.WorldInterface;
using Whenever.HealthExt.StatusEffects;
using Whenever.HealthExt.World;

namespace Whenever.HealthExt
{
    public class HealthWorld : IInspectWorldHealth, ICommandWorldHealth
    {
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

        public List<InitiatedCommand<ICommandWorldHealth>> ApplyAllStatusEffects()
        {
            var resultantCommands = new List<InitiatedCommand<ICommandWorldHealth>>();
            foreach (var combatant in allCombatants)
            {
                resultantCommands.AddRange(combatant.Value.ApplyStatusEffects(combatant.Key));
            }

            return resultantCommands;
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