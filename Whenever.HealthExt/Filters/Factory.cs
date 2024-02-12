using Whenever.Core.WheneverFilter;
using Whenever.HealthExt.World;

namespace Whenever.HealthExt.Filters
{
    public static class Factory
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
        
        public static IWheneverFilter<IInspectWorldHealth, ICommandWorldHealth> Compose(params IWheneverFilter<IInspectWorldHealth, ICommandWorldHealth>[] filters)
        {
            return new CompositeWheneverFilter<IInspectWorldHealth, ICommandWorldHealth>(filters);
        }
    }
}