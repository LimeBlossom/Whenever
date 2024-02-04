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
        
        public static IWheneverFilter CreateDotStatusEffectInflictedFilter(
            DamageType dotDamageType,
            WheneverCombatantTypeFilter wheneverCombatantTypeFilterType)
        {
            return new WheneverStatusInflicted()
            {
                dotDamageType = dotDamageType,
                wheneverCombatantTypeFilterType = wheneverCombatantTypeFilterType
            };
        }
        
    }
}