using DefaultNamespace;
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
            var filters =
                composer.ForceRegenerateComposites(
                    HealthFac.Filters.TargetHasAtLeastHealth(5),
                    HealthFac.Filters.CreateDamageOccursFilter(1)
                );
var effects = HealthFac.Effects.DamageTarget(2);
            var whenever = new Whenever<IInspectWorldHealth, ICommandWorldHealth>(filters, effects);
            Assert.AreEqual("whenever a target with at least 5 health takes 1 damage; deal 2 damage to the target", whenever.Describe());
        }
    }
    
}