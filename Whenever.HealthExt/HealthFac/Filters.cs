using HealthExtInternal;

namespace HealthFac
{
    internal static class Filters
    {
        public static IWheneverFilter<IInspectWorldHealth, ICommandWorldHealth> CreateDotStatusEffectInflictedFilter(
            float atLeastDamagePerTurn)
        {
            return new CompositeWheneverFilter<IInspectWorldHealth, ICommandWorldHealth>(
                new DotStatusIsMoreThan(atLeastDamagePerTurn)
            );
        }
        
        public static IWheneverFilter<IInspectWorldHealth, ICommandWorldHealth> CreateDamageOccursFilter(float atLeast)
        {
            return new CompositeWheneverFilter<IInspectWorldHealth, ICommandWorldHealth>(
                new DamageOccurs(atLeast)
            );
        }

        public static IWheneverFilter<IInspectWorldHealth, ICommandWorldHealth> TargetHasAtLeastHealth(float atLeast)
        {
            return new TargetHasAtLeastHealth(atLeast);
        }
    }
}