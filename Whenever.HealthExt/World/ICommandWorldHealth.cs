using Whenever.Core;
using Whenever.Core.WorldInterface;
using Whenever.HealthExt.StatusEffects;

namespace Whenever.HealthExt.World
{
    public interface ICommandWorldHealth : ICommandWorld
    {
        public void DoDamage(CombatantId id, float health);
        public void AddStatusEffect(CombatantId id, StatusEffect effect);
    }
}