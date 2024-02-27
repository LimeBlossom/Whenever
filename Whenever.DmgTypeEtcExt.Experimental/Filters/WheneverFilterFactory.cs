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
            return Compose(
                DamageIs(validDamageType),
                TargetIs(wheneverCombatantTypeFilterType)
            );
        }

        public static FilterType DamageIs(DamageType validDamageType)
        {
            return new DamageIsOfType(validDamageType);
        }

        public static FilterType CreateDealsDamageFilter(
            DamageType validDamageType,
            WheneverCombatantTypeFilter wheneverCombatantTypeFilterType)
        {
            return Compose(
                DamageIs(validDamageType),
                InitiatorIs(wheneverCombatantTypeFilterType)
            );
        }

        public static FilterType TargetIs(WheneverCombatantTypeFilter wheneverCombatantTypeFilterType)
        {
            return new CombatantIsOfType(StandardAliases.Target, wheneverCombatantTypeFilterType);
        }
        public static FilterType TargetIs(CombatantAlias alias)
        {
            return new CombatantsAreSame<IInspectableWorldDemo, ICommandableWorldDemo>(alias, StandardAliases.Target);
        }

        public static FilterType InitiatorIs(WheneverCombatantTypeFilter wheneverCombatantTypeFilterType)
        {
            return new CombatantIsOfType(StandardAliases.Initiator, wheneverCombatantTypeFilterType);
        }



        public static FilterType CreateDotStatusEffectInflictedFilter(DamageType bleed, WheneverCombatantTypeFilter enemy)
        {
            throw new System.NotImplementedException();
        }
        
        public static FilterType Compose(params FilterType[] filters)
        {
            return new CompositeWheneverFilter<IInspectableWorldDemo, ICommandableWorldDemo>(filters);
        }
    }
}