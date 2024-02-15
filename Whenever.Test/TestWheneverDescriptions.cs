using HealthExtInternal;
using HealthExtInternal.DescriptionComposer;
using NUnit.Framework;

namespace Whenever.Test
{
    public class TestWheneverDescriptions
    {
        
        private WheneverDescriptionComposer<IInspectWorldHealth, ICommandWorldHealth> composer => new(
            TargetOfHealthTakesDamage.Create());

        [Test]
        public void WheneverPlayerDealsDamage_AndTargetHasAtLeastHealth__DealsMoreDamage()
        {
            var descriptionContext = new SimpleDescriptionContext("bob", "uncle");
            var filters =
                composer.ForceRegenerateComposites(
                    HealthFac.Filters.TargetHasAtLeastHealth(5),
                    HealthFac.Filters.CreateDamageOccursFilter(1)
                );
var effects = HealthFac.Effects.DamageTarget(2);
            var whenever = new Whenever<IInspectWorldHealth, ICommandWorldHealth>(filters, effects);
            Assert.AreEqual("whenever a uncle with at least 5 health takes 1 damage; deal 2 damage to uncle", whenever.Describe(descriptionContext));
        }
    }
    
}