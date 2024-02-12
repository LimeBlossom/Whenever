using Whenever.DmgTypeEtcExt.Experimental.World;

namespace Whenever.DmgTypeEtcExt.Experimental.Filters
{
    public static class WheneverFilterFactory
    {
        
        public static IWheneverFilter<IInspectableWorldDemo, ICommandableWorldDemo> CreateDealtDamageFilter(
            DamageType validDamageType,
            WheneverCombatantTypeFilter wheneverCombatantTypeFilterType)
        {
            return new CompositeWheneverFilter<IInspectableWorldDemo, ICommandableWorldDemo>(
                new DamageIsOfType(validDamageType),
                new TargetIsOfType(wheneverCombatantTypeFilterType)
            );
        }
        
        public static IWheneverFilter<IInspectableWorldDemo, ICommandableWorldDemo> CreateDealsDamageFilter(
            DamageType validDamageType,
            WheneverCombatantTypeFilter wheneverCombatantTypeFilterType)
        {
            return new CompositeWheneverFilter<IInspectableWorldDemo, ICommandableWorldDemo>(
                new DamageIsOfType(validDamageType),
                new InitiatorIsOfType(wheneverCombatantTypeFilterType)
            );
        }

        public static IWheneverFilter<IInspectableWorldDemo, ICommandableWorldDemo> CreateDotStatusEffectInflictedFilter(DamageType bleed, WheneverCombatantTypeFilter enemy)
        {
            throw new System.NotImplementedException();
        }
    }
}