using HealthExtInternal;
using NUnit.Framework;
using Serialization;

namespace Whenever.Test
{
    public class TestWheneverCompositeSerialize
    {
        private WheneverJsonSerializer<IInspectWorldHealth, ICommandWorldHealth> GetSerializer()
        {
            return new WheneverJsonSerializer<IInspectWorldHealth, ICommandWorldHealth>();
        }
        //[Test]
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
            Assert.AreEqual("at least 2 damage occurs and target has at least 6 health", filter.Describe(new SimpleDescriptionContext()));
        }
    }
}