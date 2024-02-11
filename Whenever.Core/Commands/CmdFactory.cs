using Whenever.Core.StatusEffects;
using Whenever.Core.WheneverTestDemo;
using Whenever.Core.WorldInterface;

namespace Whenever.Core.Commands
{
    public static class CmdFactory
    {
        public static IWorldCommand<ICommandableWorldDemo> Damage(DamageType type, int amount, CombatantId target)
        { 
            return new DamageCommand(target, new DamagePackage(type, amount));
        }
        
        public static IWorldCommand<ICommandableWorldDemo> Status(CombatantId target, StatusEffect effect)
        {
            return new AddStatusEffectCommand(target, effect);
        }
    }
}