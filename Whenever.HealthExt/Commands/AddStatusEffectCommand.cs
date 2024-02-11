using Whenever.Core.StatusEffects;
using Whenever.Core.WheneverTestDemo;
using Whenever.Core.WorldInterface;
using Whenever.HealthExt;

namespace Whenever.Core.Commands
{
    public record AddStatusEffectCommand: IGenericTargetedWorldCommand<ICommandWorldHealth>
    {
        public CombatantId Target { get; }
        
        public StatusEffect statusEffect;
    
        public AddStatusEffectCommand(CombatantId target, StatusEffect effect)
        {
            Target = target;
            statusEffect = effect;
        }

        public void ApplyCommand(ICommandWorldHealth world)
        {
            world.AddStatusEffect(Target, statusEffect);
        }
    }
}