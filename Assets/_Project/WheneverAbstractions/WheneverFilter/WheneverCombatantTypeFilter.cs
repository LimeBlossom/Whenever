using System;

namespace WheneverAbstractions._Project.WheneverAbstractions.WheneverFilter
{
    [Flags]
    public enum WheneverCombatantTypeFilter
    {
        Player = 1>>0,
        Enemy = 1>>1,
        Any = 0xFFFF
    }
}