using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Whenever.Core.PrimitiveUtilities;
using Whenever.Core.WorldInterface;
using Random = System.Random;

namespace Whenever.Core.WheneverTestDemo
{
    public interface IInspectableWorldDemo: IInspectWorld
    {
        public ICombatantData CombatantData(CombatantId combatantId);
        public Random GetRng();
        
        public CombatantId GetAtLocation(Vector2 location);
        
    }
    
    public static class InspectableWorldExtensions{

        public static IEnumerable<CombatantId> GetAdjacentCombatants(this IInspectableWorldDemo world, CombatantId combatantId)
        {
            var combatantData = world.CombatantData(combatantId);
            var adjacentTiles = VectorExtensions.GetAdjacentTiles(combatantData.GetPosition());
            return adjacentTiles
                .Select(world.GetAtLocation)
                .Where(x => x != CombatantId.INVALID);
        }
    }
}