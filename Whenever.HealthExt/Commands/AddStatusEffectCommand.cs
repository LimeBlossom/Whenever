using Whenever.Core.Commands;
using Whenever.Core.WorldInterface;
using Whenever.HealthExt.StatusEffects;
using Whenever.HealthExt.World;

namespace Whenever.HealthExt.Commands
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