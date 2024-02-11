using Whenever.Core.WheneverTestDemo;
using Whenever.Core.WorldInterface;

namespace Whenever.Core.WheneverFilter
{
    public static class WheneverFilterFactory
    {
        
        public static IWheneverFilter<IInspectableWorldDemo, ICommandableWorldDemo> CreateDealtDamageFilter(
            DamageType validDamageType,
            WheneverCombatantTypeFilter wheneverCombatantTypeFilterType)
        {
            return new CompositeWheneverFilter(
                new DamageIsOfType(validDamageType),
                new TargetIsOfType(wheneverCombatantTypeFilterType)
            );
        }
        
        public static IWheneverFilter<IInspectableWorldDemo, ICommandableWorldDemo> CreateDealsDamageFilter(
            DamageType validDamageType,
            WheneverCombatantTypeFilter wheneverCombatantTypeFilterType)
        {
            return new CompositeWheneverFilter(
                new DamageIsOfType(validDamageType),
                new InitiatorIsOfType(wheneverCombatantTypeFilterType)
            );
        }
        
        public static IWheneverFilter<IInspectableWorldDemo, ICommandableWorldDemo> CreateDotStatusEffectInflictedFilter(
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