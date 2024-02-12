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
            
            Assert.AreEqual("Could not find effect type SomeTypeThatsNotDeclared", error);
        }
    }
}