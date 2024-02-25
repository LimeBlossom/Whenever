using Whenever.DmgTypeEtcExt.Experimental.World;

namespace Whenever.DmgTypeEtcExt.Experimental.Filters
{
    using FilterType = IWheneverFilter<IInspectableWorldDemo, ICommandableWorldDemo>;
    public static class WheneverFilterFactory
    {
        
        public static FilterType CreateDealtDamageFilter(
            DamageType validDamageType,
            WheneverCombatantTypeFilter wheneverCombatantTypeFilterType)
        {
            return new CompositeWheneverFilter<IInspectableWorldDemo, ICommandableWorldDemo>(
                DamageIs(validDamageType),
                TargetIs(wheneverCombatantTypeFilterType)
            );
        }

        private static FilterType DamageIs(DamageType validDamageType)
        {
            return new DamageIsOfType(validDamageType);
        }

        public static FilterType CreateDealsDamageFilter(
            DamageType validDamageType,
            WheneverCombatantTypeFilter wheneverCombatantTypeFilterType)
        {
            return new CompositeWheneverFilter<IInspectableWorldDemo, ICommandableWorldDemo>(
                DamageIs(validDamageType),
                InitiatorIs(wheneverCombatantTypeFilterType)
            );
        }

        private static FilterType TargetIs(WheneverCombatantTypeFilter wheneverCombatantTypeFilterType)
        {
            return new CombatantIsOfType(StandardAliases.Target, wheneverCombatantTypeFilterType);
        }

        private static FilterType InitiatorIs(WheneverCombatantTypeFilter wheneverCombatantTypeFilterType)
        {
            return new CombatantIsOfType(StandardAliases.Initiator, wheneverCombatantTypeFilterType);
        }


        public static FilterType CreateDotStatusEffectInflictedFilter(DamageType bleed, WheneverCombatantTypeFilter enemy)
        {
            throw new System.NotImplementedException();
        }
    }
}