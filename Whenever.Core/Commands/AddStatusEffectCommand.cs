using Whenever.Core.StatusEffects;
using Whenever.Core.WheneverTestDemo;
using Whenever.Core.WorldInterface;

namespace Whenever.Core.Commands
{
    public record AddStatusEffectCommand: ITargetedWorldCommand
    {
        public CombatantId Target { get; }
        
        public StatusEffect statusEffect;
    
        public AddStatusEffectCommand(CombatantId target, StatusEffect effect)
        {
            Target = target;
            statusEffect = effect;
        }

        public void ApplyCommand(ICommandableWorldDemo world)
        {
            var target = world.GetCombatantRaw(Target);
            target.statusEffects.Add(statusEffect);
        }
    }
}