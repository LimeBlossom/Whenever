using System.Collections.Generic;
using Serialization;

namespace HealthExtInternal
{
    [PolymorphicSerializable("DamageTargetEffect")]
    internal record DamageTargetEffect : EffectTargetEffect<IInspectWorldHealth,ICommandWorldHealth>
    {
        public float damage;
        
        protected override string DescribeOnTarget()
        {
            return $"deal {damage} damage";
        }

        protected override IEnumerable<IWorldCommand<ICommandWorldHealth>> ApplyEffectToTarget(CombatantId target, IInspectWorldHealth world)
        {
            yield return new Damage(target, damage);
        }
    }
}