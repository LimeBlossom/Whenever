using System;

namespace WheneverAbstractions._Project.WheneverAbstractions.WheneverFilter
{
    public static class WheneverFilterFactory
    {
        
        public static IWheneverFilter CreateDealtDamageFilter(
            DamageType validDamageType,
            WheneverCombatantTypeFilter wheneverCombatantTypeFilterType)
        {
            return new WheneverDealtDamage
            {
                validDamageType = validDamageType,
                wheneverCombatantTypeFilterType = wheneverCombatantTypeFilterType
            };
        }
        
        public static IWheneverFilter CreateDealsDamageFilter(
            DamageType validDamageType,
            WheneverCombatantTypeFilter wheneverCombatantTypeFilterType)
        {
            return new WheneverDealsDamage
            {
                validDamageType = validDamageType,
                wheneverCombatantTypeFilterType = wheneverCombatantTypeFilterType
            };
        }
        
        public static IWheneverFilter CreateStatusEffectInflictedFilter(
            Type statusEffectType,
            WheneverCombatantTypeFilter wheneverCombatantTypeFilterType)
        {
            return new WheneverStatusInflicted()
            {
                statusEffectType = statusEffectType,
                wheneverCombatantTypeFilterType = wheneverCombatantTypeFilterType
            };
        }
        
    }
}