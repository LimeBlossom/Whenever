using System.Collections.Generic;
using System.Linq;
using HealthExtInternal;
using HealthExtInternal.DescriptionComposer;
using NUnit.Framework;

namespace Whenever.Test
{
    public class TestWheneverDescriptions
    {
        private WheneverDescriptionComposer<IInspectWorldHealth, ICommandWorldHealth> composer => 
            new(TargetOfHealthTakesDamage.CreateAll().ToArray());

        [Test]
        public void WheneverPlayerDealsDamage_AndTargetHasAtLeastHealth__DealsMoreDamage()
        {
            var descriptionContext = SimpleDescriptionContext.CreateInstance("bob", "uncle");
            var filters =
                composer.ForceRegenerateComposites(
                    HealthFac.Filters.TargetHasAtLeastHealth(5),
                    HealthFac.Filters.CreateDamageOccursFilter(1)
                );
var effects = HealthFac.Effects.DamageTarget(2);
            var whenever = new Whenever<IInspectWorldHealth, ICommandWorldHealth>(filters, effects);
            Assert.AreEqual("When uncle with at least 5 health takes 1 damage; deal 2 damage to uncle", whenever.Describe(descriptionContext));
        }

        [Test]
        public void PlayerDealsDamage_EmptyTargetName_OmitsTargetAsSubject()
        {
            var descriptionContext = SimpleDescriptionContext.CreateInstance("George", "");
            var effect = HealthFac.Effects.DamageTarget(2);
            
            Assert.AreEqual("deal 2 damage", effect.Describe(descriptionContext));
        }
        
        [Test]
        public void DealsDamageToSpecificTarget_WithEmptyTargetName_OmitsTargetAsSubject()
        {
            var descDict = new Dictionary<CombatantId, string>
            {
                {CombatantId.Hashed("#bob"), "" },
                {CombatantId.Hashed("#george"), "George" }
            };
            var descriptionContext = SimpleDescriptionContext.CreateInstance("George", "", descDict);
            
            var effectToEmptyTarget = HealthFac.Effects.DamageSpecificTarget(2, CombatantId.Hashed("#bob"));
            Assert.AreEqual("deal 2 damage", effectToEmptyTarget.Describe(descriptionContext));
            
            var effectToGeorge = HealthFac.Effects.DamageSpecificTarget(2, CombatantId.Hashed("#george"));
            Assert.AreEqual("deal 2 damage to George", effectToGeorge.Describe(descriptionContext));
        }
    }
}