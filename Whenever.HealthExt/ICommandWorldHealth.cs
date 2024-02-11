using Whenever.Core.WorldInterface;

namespace Whenever.HealthExt
{
    public interface ICommandWorldHealth : ICommandWorld
    {
        public void SetHealth(CombatantId id, float health);
    }
}