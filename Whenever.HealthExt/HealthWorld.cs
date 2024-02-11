using System.Collections.Generic;
using Whenever.Core.WorldInterface;

namespace Whenever.HealthExt
{
    
    public class HealthWorld : CoreActorWorld<HealthCombatant>, IInspectWorldHealth, ICommandWorldHealth
    {
        public HealthWorld(List<HealthCombatant> allCombatants) : base(allCombatants)
        {
        }

        public float GetHealth(CombatantId id)
        {
            return InspectCombatant(id).health;
        }

        public void SetHealth(CombatantId id, float health)
        {
            var combatant = InspectCombatant(id);
            combatant.health = health;
        }
    }

    public class HealthCombatant
    {
        public float health;
        
        public HealthCombatant(float health)
        {
            this.health = health;
        }
    }
}