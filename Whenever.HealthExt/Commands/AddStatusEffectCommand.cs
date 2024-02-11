using Whenever.Core;
using Whenever.Core.Commands;
using Whenever.HealthExt.StatusEffects;
using Whenever.HealthExt.World;

namespace Whenever.HealthExt.Commands
{
    public record AddStatusEffectCommand: IGenericTargetedWorldCommand<ICommandWorldHealth>
    {
        public CombatantId Target { get; }
        
        public StatusEffect<ICommandWorldHealth> statusEffect;
    
        public AddStatusEffectCommand(CombatantId target, StatusEffect<ICommandWorldHealth> effect)
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