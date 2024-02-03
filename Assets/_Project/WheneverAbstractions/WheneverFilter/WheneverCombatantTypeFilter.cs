using System;

namespace WheneverAbstractions._Project.WheneverAbstractions.WheneverFilter
{
    [Flags]
    public enum WheneverCombatantTypeFilter
    {
        Player = 1<<0,
        Enemy = 1<<1,
        Environment = 1<<2,
        Any = 0xFFFF
    }
}