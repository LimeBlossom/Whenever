using HealthExtInternal;
using NUnit.Framework;
using Serialization;

namespace Whenever.Test
{
    public class TestWheneverBasicSerialize
    {
        private WheneverJsonSerializer<IInspectWorldHealth, ICommandWorldHealth> GetSerializer()
        {
            return new WheneverJsonSerializer<IInspectWorldHealth, ICommandWorldHealth>();
        }

        [Test]
        public void DeserializesDamageTargetEffect()
        {
            var serializer = GetSerializer();
            
            var json = @"
            {
                ""type"": ""DamageCombatantEffect"",
                ""damage"": 3,
                ""combatant"": {""alias"": ""#target""}
            }
            ";
            
            var (effect, error) = serializer.DeserializeEffect(json);
            Assert.IsNull(error);
            Assert.IsNotNull(effect);
            Assert.AreEqual("deal 3 damage to the target", effect.Describe(SimpleDescriptionContext.CreateInstance()));
            Assert.AreEqual(typeof(DamageCombatantEffect), effect.GetType());
            Assert.AreEqual(StandardAliases.Target,(effect as DamageCombatantEffect)?.CombatantTarget);
        }
        
        [Test]
        public void DeserializedWithCustomCombatantAlias()
        {
            var serializer = GetSerializer();
            
            var json = @"
            {
                ""type"": ""DamageCombatantEffect"",
                ""damage"": 3,
                ""combatant"": {""alias"": ""#cardTargetCustom"" }
            }
            ";
            
            var descriptionContext = SimpleDescriptionContext.CreateInstance();
            descriptionContext = descriptionContext.WithOverrideWhenNotDefined(
                CombatantAlias.FromId("#cardTargetCustom"),
                "the custom card target name",
                FallbackConstants.OverrideBaseAliasNameL1);
            
            
            var (effect, error) = serializer.DeserializeEffect(json);
            Assert.IsNull(error);
            Assert.IsNotNull(effect);
            Assert.AreEqual("deal 3 damage to the custom card target name", effect.Describe(descriptionContext));
            Assert.AreEqual(typeof(DamageCombatantEffect), effect.GetType());
            Assert.AreEqual(CombatantAlias.FromId("#cardTargetCustom"),(effect as DamageCombatantEffect)?.CombatantTarget);
        }

        [Test]
        public void DeserializesDotStatusTargetEffect()
        {
            var serializer = GetSerializer();
            
            var json = @"
            {
                ""type"": ""DotCombatantEffect"",
                ""damage"": 1,
                ""turns"": 3,
                ""combatant"": {""alias"": ""#target""}
            }
            ";
            
            var (effect, error) = serializer.DeserializeEffect(json);
            Assert.IsNull(error);
            Assert.IsNotNull(effect);
            Assert.AreEqual("apply 1 damage per turn for 3 turns to the target", effect.Describe(SimpleDescriptionContext.CreateInstance()));
            Assert.AreEqual(typeof(DotCombatantEffect), effect.GetType());
            Assert.AreEqual(StandardAliases.Target,(effect as DotCombatantEffect)?.CombatantTarget);
        }
        

        [Test]
        public void DeserializesInvalidTypeNameToError()
        {
            var serializer = GetSerializer();
            
            var json = @"
            {
                ""type"": ""SomeTypeThatsNotDeclared"",
                ""someNumber"": 1
            }
            ";
            
            var (effect, error) = serializer.DeserializeEffect(json);
            Assert.IsNotNull(error);
            Assert.IsNull(effect);
            
            Assert.AreEqual("Could not find type SomeTypeThatsNotDeclared", error);
        }
        
        [Test]
        public void DeserializesInvalidJsonToError()
        {
            var serializer = GetSerializer();
            
            var json = @"
            {
                ""type"": ""SomeTypeThatsNotDeclared""
                ""someNumber"": 1
            }
            ";
            
            var (effect, error) = serializer.DeserializeEffect(json);
            Assert.IsNotNull(error);
            Assert.IsNull(effect);
            
            Assert.AreEqual("JSON parse error: Missing a comma or '}' after an object member.", error);
        }

        [Test]
        public void DeserializesDotStatusTargetEffect_WithMissingProperties__DeserializedPropertyToDefault()
        {
            var serializer = GetSerializer();
            
            var json = @"
            {
                ""type"": ""DotCombatantEffect"",
                ""turns"": 3,
                ""combatant"": {""alias"": ""#target""}
            }
            ";
            
            var (effect, error) = serializer.DeserializeEffect(json);
            Assert.IsNull(error);
            Assert.IsNotNull(effect);
            Assert.AreEqual("apply 0 damage per turn for 3 turns to the target", effect.Describe(SimpleDescriptionContext.CreateInstance()));
            Assert.AreEqual(typeof(DotCombatantEffect), effect.GetType());
            Assert.AreEqual(StandardAliases.Target,(effect as DotCombatantEffect)?.CombatantTarget);
        }

        [Test]
        public void DeserializesWheneverDamageOccursFilter()
        {
            var serializer = GetSerializer();

            var json = @"
            {
                ""type"": ""DamageOccurs"",
                ""atLeast"": 5
            }";
            
            var (filter, error) = serializer.DeserializeFilter(json);
            Assert.IsNull(error);
            Assert.IsNotNull(filter);
            Assert.AreEqual("at least 5 damage occurs", filter.Describe(SimpleDescriptionContext.CreateInstance()));
            Assert.AreEqual(typeof(DamageOccurs), filter.GetType());
        }
        
        [Test]
        public void DeserializesWheneverTargetHasAtLeastHealthFilter()
        {
            var serializer = GetSerializer();

            var json = @"
            {
                ""type"": ""CombatantHasAtLeastHealth"",
                ""atLeast"": 5,
                ""combatant"": {""alias"": ""#target""}
            }";
            
            var (filter, error) = serializer.DeserializeFilter(json);
            Assert.IsNull(error);
            Assert.IsNotNull(filter);
            Assert.AreEqual("the target has at least 5 health", filter.Describe(SimpleDescriptionContext.CreateInstance()));
            Assert.AreEqual(typeof(CombatantHasAtLeastHealth), filter.GetType());
            Assert.AreEqual(StandardAliases.Target,(filter as CombatantHasAtLeastHealth)?.combatant);
        }
        
    }
}