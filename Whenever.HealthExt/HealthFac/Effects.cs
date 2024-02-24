using HealthExtInternal;

namespace HealthFac
{
    internal class Effects
    {
        public static IEffect<IInspectWorldHealth, ICommandWorldHealth> DotTarget(float damage = 1, int turns = 3)
        {
            return new DotStatusTargetEffect
            {
                damage = damage,
                turns = turns
            };
        }
        
        public static IEffect<IInspectWorldHealth, ICommandWorldHealth> DamageTarget(float damage)
        {
            return new DamageTargetEffect
            {
                damage = damage
            };
        }
        
        public static IEffect<IInspectWorldHealth, ICommandWorldHealth> DamageSpecificTarget(float damage, CombatantId specificTarget)
        {
            return new DamageSpecificEffect(specificTarget)
            {
                damage = damage
            };
        }
    }
}