using Whenever.Core.Commands;
using Whenever.Core.WorldInterface;
using Whenever.HealthExt.StatusEffects;
using Whenever.HealthExt.World;

namespace Whenever.HealthExt.Commands
{
    public static class Factory
    {
        public static IWorldCommand<ICommandWorldHealth> Status(CombatantId target, StatusEffect effect)
        {
            return new AddStatusEffectCommand(target, effect);
        }
        
        public static IWorldCommand<ICommandWorldHealth> Damage(CombatantId target, float damage)
        {
            return new Damage(target, damage);
        }
    }
}