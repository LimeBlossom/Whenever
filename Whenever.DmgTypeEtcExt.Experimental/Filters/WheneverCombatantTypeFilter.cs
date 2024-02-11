using System;

namespace Whenever.DmgTypeEtcExt.Experimental.Filters
{
    [Flags]
    public enum WheneverCombatantTypeFilter
    {
        Player = 1<<0,
        Enemy = 1<<1,
        Environment = 1<<2,
        Any = 0xFFFF
    }
    
    public static class WheneverCombatantTypeExtensions{
    
        public static WheneverCombatantTypeFilter ToTypeFilter(this CombatantType combatantType)
        {
            return combatantType switch
            {
                CombatantType.Player => WheneverCombatantTypeFilter.Player,
                CombatantType.Enemy => WheneverCombatantTypeFilter.Enemy,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}