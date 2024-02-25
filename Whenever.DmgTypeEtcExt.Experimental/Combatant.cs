using UnityEngine;

namespace Whenever.DmgTypeEtcExt.Experimental
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
        public CombatantType combatantType;
        public Vector2 position;

        public Combatant(int maxHealth, CombatantType type, Vector2 position = default)
        {
            this.health = new Health(maxHealth);
            this.damageable = new Damageable();
            this.combatantType = type;
        
            this.position = position;
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