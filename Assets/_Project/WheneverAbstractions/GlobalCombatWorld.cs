using System;
using System.Collections.Generic;
using WheneverAbstractions._Project.WheneverAbstractions.Commands;

namespace WheneverAbstractions._Project.WheneverAbstractions
{
    public class GlobalCombatWorld
    {
        private List<Whenever> whenevers = new();
        private Dictionary<CombatantId, Combatant> allCombatants;
        
        public GlobalCombatWorld(List<Combatant> allCombatants)
        {
            this.allCombatants = new();
            var id = CombatantId.DEFAULT;
            foreach (var combatant in allCombatants)
            {
                id = CombatantId.Next(id);
                this.allCombatants[id] = combatant;
            }
            
        }

        public IEnumerable<CombatantId> GetPlayers()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CombatantId> GetEnemies()
        {
            throw new NotImplementedException();
        }
        
        public ICombatantData CombatantData(CombatantId combatantId)
        {
            return allCombatants[combatantId];
        }
        public void StartPlayerTurn()
        {
            throw new NotImplementedException();
        }

        public void StartEnemyTurn()
        {
            throw new NotImplementedException();
        }

        public void InitiateCommand(IWorldCommand command, ICommandInitiator initiator)
        {
            
        }
    }
}