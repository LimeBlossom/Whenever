using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Whenever.Core.WorldInterface;
using Whenever.DmgTypeEtcExt.Experimental.PrimitiveUtilities;
using Random = System.Random;

namespace Whenever.DmgTypeEtcExt.Experimental.World
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