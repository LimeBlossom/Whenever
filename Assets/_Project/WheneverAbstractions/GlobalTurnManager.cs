using System.Collections.Generic;
using System.Linq;

namespace _Project.WheneverAbstractions
{
    public class GlobalTurnManager
    {
        private WheneverManager whenevers;
        private List<Combatant> allCombatants;
        
        public GlobalTurnManager(WheneverManager whenevers, List<Combatant> allCombatants)
        {
            this.whenevers = whenevers;
            this.allCombatants = allCombatants;
        }
        
        public void AddWhenever(Whenever whenever)
        {
            whenevers.AddWhenever(whenever);
        }


        public void StartPlayerTurn()
        {
            foreach (Combatant combatant in allCombatants.Where(combatant => combatant.combatantType == CombatantType.Player))
            {
                combatant.StartTurn();
            }
        }

        public void StartEnemyTurn()
        {
            foreach (Combatant combatant in allCombatants.Where(combatant => combatant.combatantType == CombatantType.Enemy))
            {
                combatant.StartTurn();
            }
        }

        public void ApplyDamage(DamagePackage damage)
        {
            damage.target.damageable.TakeDamage(ref damage);
            whenevers.CheckWhenevers(damage, allCombatants);
        }
    }
}