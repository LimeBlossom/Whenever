﻿namespace WheneverAbstractions._Project.WheneverAbstractions.WheneverFilter
{
    public static class WheneverFilterFactory
    {
        
        public static IWheneverFilter CreateDealtDamageFilter(
            DamageType validDamageType,
            WheneverCombatantTypeFilter wheneverCombatantTypeFilterType)
        {
            return new CompositeWheneverFilter(
                new DamageIsOfType(validDamageType),
                new TargetIsOfType(wheneverCombatantTypeFilterType)
            );
        }
        
        public static IWheneverFilter CreateDealsDamageFilter(
            DamageType validDamageType,
            WheneverCombatantTypeFilter wheneverCombatantTypeFilterType)
        {
            return new CompositeWheneverFilter(
                new DamageIsOfType(validDamageType),
                new InitiatorIsOfType(wheneverCombatantTypeFilterType)
            );
        }
        
        public static IWheneverFilter CreateDotStatusEffectInflictedFilter(
            DamageType dotDamageType,
            WheneverCombatantTypeFilter wheneverCombatantTypeFilterType)
        {
            return new CompositeWheneverFilter(
                new DotStatusIsOfType(dotDamageType),
                new TargetIsOfType(wheneverCombatantTypeFilterType)
            );
        }
        
    }
}