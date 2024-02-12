using NUnit.Framework;
using Serialization;

namespace Whenever.Test
{
    public class TestWheneverSerialize
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
            Assert.AreEqual("deal 3 damage to the target", effect.Describe());
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
            Assert.AreEqual("apply 1 damage per turn for 3 turns to the target", effect.Describe());
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
            Assert.AreEqual("apply 0 damage per turn for 3 turns to the target", effect.Describe());
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
            Assert.AreEqual("at least 5 damage occurs", filter.Describe());
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
            Assert.AreEqual("target has at least 5 health", filter.Describe());
            Assert.AreEqual(typeof(TargetHasAtLeastHealth), filter.GetType());
        }
        
        
        [Test]
        public void DeserializesCompositeWhenever_DamageOccurs_AndTargetHasAtLeastHealth()
        {
            var serializer = GetSerializer();

            var json = @"
            {
                ""type"": ""CompositeWhenever"",
                ""description"": ""something"",
                ""filters"": [
                    {""type"": ""DamageOccurs"", ""atLeast"": 2},
                    {""type"": ""TargetHasAtLeastHealth"", ""atLeast"": 6}
                ]
            }";
            
            var (filter, error) = serializer.DeserializeFilter(json);
            Assert.IsNull(error);
            Assert.IsNotNull(filter);
            Assert.AreEqual(typeof(CompositeWheneverFilter<IInspectWorldHealth, ICommandWorldHealth>), filter.GetType());
            Assert.AreEqual("at least 2 damage occurs and target has at least 6 health", filter.Describe());
        }
    }
}