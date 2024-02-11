using Whenever.Core.WheneverFilter;
using Whenever.Core.WorldInterface;
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
        
    }
}