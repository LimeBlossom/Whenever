namespace HealthExtInternal.DescriptionComposer
{
    internal static class TargetOfHealthTakesDamage
    {
        public static WheneverCompositeDescription<IInspectWorldHealth, ICommandWorldHealth> Create()
        {
            return WheneverCompositeDescriptionFactory<IInspectWorldHealth, ICommandWorldHealth>
                .Create<DamageOccurs, TargetHasAtLeastHealth>((ctx, dmgOccurs, atLeastHealth) => 
                    $"a {ctx.TargetName} with at least {atLeastHealth.atLeast} health takes {dmgOccurs.atLeast} damage");
        }
    }
}