using Whenever.Core.WorldInterface;

namespace Whenever.HealthExt.Effects
{
    public class Factory
    {
        public static IEffect<IInspectWorldHealth, ICommandWorldHealth> DotTarget(float damage = 1, int turns = 3)
        {
            return new DotStatusTargetEffect
            {
                damage = damage,
                turns = turns
            };
        }
    }
}