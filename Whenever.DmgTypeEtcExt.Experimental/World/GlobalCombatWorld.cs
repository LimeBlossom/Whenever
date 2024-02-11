using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Whenever.Core;
using Whenever.Core.WorldInterface;
using Random = System.Random;

namespace Whenever.DmgTypeEtcExt.Experimental.World
{
    public class GlobalCombatWorldDemo : IInspectableWorldDemo, ICommandableWorldDemo
    {
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

        public void SaySomething(CombatantId id, string message)
        {
            throw new NotImplementedException();
        }
    }

}