using Whenever.Core.StatusEffects;
using Whenever.Core.WorldInterface;

namespace Whenever.HealthExt
{
    public interface ICommandWorldHealth : ICommandWorld
    {
        public void DoDamage(CombatantId id, float health);
        public void AddStatusEffect(CombatantId id, StatusEffect effect);
    }
}