using System.Collections.Generic;

namespace WheneverAbstractions._Project.WheneverAbstractions
{
    public class WheneverManager
    {
        private List<Whenever> whenevers = new();
    
        public void AddWhenever(Whenever whenever)
        {
            whenevers.Add(whenever);
        }

        public void CheckWhenevers(DamagePackage damagePackage, IEnumerable<Combatant> allCombatants)
        {
            foreach(Combatant combatant in allCombatants)
            {
                foreach(Whenever whenever in whenevers)
                {
                    whenever.TryTrigger(damagePackage, combatant);
                }
            }
        }

        public void Clear()
        {
            whenevers.Clear();
        }
    }
}
