using System.Collections.Generic;
using static WheneverCompositeDescriptionFactory<HealthExtInternal.IInspectWorldHealth,HealthExtInternal.ICommandWorldHealth>;

namespace HealthExtInternal.DescriptionComposer
{
    internal static class TargetOfHealthTakesDamage
    {
        public static IEnumerable<WheneverCompositeDescription<IInspectWorldHealth, ICommandWorldHealth>> CreateAll()
        {
            yield return Create<DamageOccurs, CombatantHasAtLeastHealth>((ctx, dmgOccurs, atLeastHealth) =>
                $"{ctx.NameOf(atLeastHealth.combatant)} with at least {atLeastHealth.atLeast} health takes {dmgOccurs.atLeast} damage");
                
        }
    }
}