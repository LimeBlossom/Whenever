using System.Collections.Generic;
using UnityEngine;
using Whenever.Core.CommandInitiators;
using Whenever.Core.StatusEffects;
using Whenever.Core.WheneverTestDemo;
using Whenever.Core.WorldInterface;

namespace Whenever.Core
{
    public enum CombatantType
    {
        Player,
        Enemy
    }

    public interface ICombatantData
    {
        public float CurrentHealth();
        public Vector2 GetPosition();
        public CombatantType GetCombatantType();
    }
    
    public class Combatant : ICombatantData
    {
        public Health health;
        public Damageable damageable;
        public List<StatusEffect> statusEffects = new();
        public CombatantType combatantType;
        public Vector2 position;

        public Combatant(int maxHealth, CombatantType type, Vector2 position = default)
        {
            this.health = new Health(maxHealth);
            this.damageable = new Damageable();
            this.combatantType = type;
        
            this.position = position;
        }
    
        public IEnumerable<InitiatedCommand<ICommandableWorldDemo>> ApplyStatusEffects(CombatantId myId)
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
                    yield return new InitiatedCommand<ICommandableWorldDemo>(command, InitiatorFactory.From(statusEffect));
                }
            }
        }

        public float CurrentHealth()
        {
            return health.GetCurrentHealth();
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public CombatantType GetCombatantType()
        {
            return combatantType;
        }
    }
}