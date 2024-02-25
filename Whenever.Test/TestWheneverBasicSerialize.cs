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
                ""type"": ""DamageTargetEffect"",
                ""damage"": 3
            }
            ";
            
            var (effect, error) = serializer.DeserializeEffect(json);
            Assert.IsNull(error);
            Assert.IsNotNull(effect);
            Assert.AreEqual("deal 3 damage to the target", effect.Describe(new SimpleDescriptionContext()));
            Assert.AreEqual(typeof(DamageTargetEffect), effect.GetType());
        }

        [Test]
        public void DeserializesDotStatusTargetEffect()
        {
            var serializer = GetSerializer();
            
            var json = @"
            {
                ""type"": ""DotStatusTargetEffect"",
                ""damage"": 1,
                ""turns"": 3
            }
            ";
            
            var (effect, error) = serializer.DeserializeEffect(json);
            Assert.IsNull(error);
            Assert.IsNotNull(effect);
            Assert.AreEqual("apply 1 damage per turn for 3 turns to the target", effect.Describe(new SimpleDescriptionContext()));
            Assert.AreEqual(typeof(DotStatusTargetEffect), effect.GetType());
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
                ""type"": ""DotStatusTargetEffect"",
                ""turns"": 3
            }
            ";
            
            var (effect, error) = serializer.DeserializeEffect(json);
            Assert.IsNull(error);
            Assert.IsNotNull(effect);
            Assert.AreEqual("apply 0 damage per turn for 3 turns to the target", effect.Describe(new SimpleDescriptionContext()));
            Assert.AreEqual(typeof(DotStatusTargetEffect), effect.GetType());
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
            Assert.AreEqual("at least 5 damage occurs", filter.Describe(new SimpleDescriptionContext()));
            Assert.AreEqual(typeof(DamageOccurs), filter.GetType());
        }
        
        [Test]
        public void DeserializesWheneverTargetHasAtLeastHealthFilter()
        {
            var serializer = GetSerializer();

            var json = @"
            {
                ""type"": ""TargetHasAtLeastHealth"",
                ""atLeast"": 5
            }";
            
            var (filter, error) = serializer.DeserializeFilter(json);
            Assert.IsNull(error);
            Assert.IsNotNull(filter);
            Assert.AreEqual("the target has at least 5 health", filter.Describe(new SimpleDescriptionContext()));
            Assert.AreEqual(typeof(TargetHasAtLeastHealth), filter.GetType());
        }
    }
}