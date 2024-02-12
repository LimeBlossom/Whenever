using System.Collections.Generic;
using Whenever.Core;
using Whenever.Core.Effects;
using Whenever.Core.WorldInterface;
using Whenever.HealthExt.Commands;
using Whenever.HealthExt.World;

namespace Whenever.HealthExt.Effects
{
    public record DamageTargetEffect : EffectTargetEffect<IInspectWorldHealth,ICommandWorldHealth>
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