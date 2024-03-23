using System.Collections.Generic;
using Serialization;

namespace HealthExtInternal
{
    [PolymorphicSerializable("DamageSpecificEffect")]
    internal record DamageSpecificEffect : EffectSpecificTargetEffect<IInspectWorldHealth,ICommandWorldHealth>
    {
        public float damage;
        
        public DamageSpecificEffect(CombatantId specificTarget) : base(specificTarget)
        {
        }
        
        protected override string DescribeEffect()
        {
            return $"deal {damage} damage";
        }

        protected override IEnumerable<IWorldCommand<ICommandWorldHealth>> ApplyEffectTo(CombatantId target, IInspectWorldHealth world)
        {
            yield return new Damage(target, damage);
        }

    }
}