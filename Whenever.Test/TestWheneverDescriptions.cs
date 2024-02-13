using System.Linq;
using HealthExtInternal;
using HealthFac;
using NUnit.Framework;
using Filters = CoreFac.Filters;
using Initiators = CoreFac.Initiators;

namespace Whenever.Test
{
    using WheneverType = Whenever<IInspectWorldHealth, ICommandWorldHealth>;

    public class TestWheneverDescriptions
    {

        [Test]
        public void WheneverPlayerDealsDamage_AndTargetHasAtLeastHealth__DealsMoreDamage()
        {
            var filters =
                Filters.Compose(
                    HealthFac.Filters.TargetHasAtLeastHealth(5),
                    HealthFac.Filters.CreateDamageOccursFilter(1)
                );
var effects = HealthFac.Effects.DamageTarget(2);
            var whenever = new Whenever<IInspectWorldHealth, ICommandWorldHealth>(filters, effects);
        }
    }
    
}