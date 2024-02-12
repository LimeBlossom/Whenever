namespace HealthFac
{
    public class Effects
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
    }
}