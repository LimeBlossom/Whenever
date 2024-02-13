using DefaultNamespace;

namespace HealthExtInternal.DescriptionComposer
{
    internal class TargetOfHealthTakesDamage: WheneverCompositeDescription<IInspectWorldHealth, ICommandWorldHealth>
    {
        public TargetOfHealthTakesDamage() : base(typeof(DamageOccurs), typeof(TargetHasAtLeastHealth))
        {
        }
        
        protected override string DescribeMatch(IWheneverFilter<IInspectWorldHealth, ICommandWorldHealth>[] consumedFilters)
        {
            var dmgOccurs = consumedFilters[0] as DamageOccurs;
            var atLeastHealth = consumedFilters[1] as TargetHasAtLeastHealth;

            return $"a target with at least {atLeastHealth!.atLeast} health takes {dmgOccurs!.atLeast} damage";
        }
    }
}