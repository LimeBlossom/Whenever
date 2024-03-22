using System;
using HealthExtInternal;
using static StandardAliases;

namespace HealthFac
{
    using FilterType = IWheneverFilter<IInspectWorldHealth, ICommandWorldHealth>;
    internal static class Filters
    {
        public static FilterType CreateDotStatusEffectInflictedFilter(
            float atLeastDamagePerTurn)
        {
            return new CompositeWheneverFilter<IInspectWorldHealth, ICommandWorldHealth>(
                new DotStatusIsMoreThan(atLeastDamagePerTurn)
            );
        }
        
        public static FilterType CreateDamageOccursFilter(float atLeast)
        {
            return new CompositeWheneverFilter<IInspectWorldHealth, ICommandWorldHealth>(
                new DamageOccurs(atLeast)
            );
        }

        public static FilterType TargetHasAtLeastHealth(float atLeast)
        {
            return HasAtLeastHealth(Target, atLeast);
        }
        
        public static FilterType HasAtLeastHealth(CombatantAlias alias, float atLeast)
        {
            return new CombatantHasAtLeastHealth(alias, atLeast);
        }
    }
}