using WheneverAbstractions._Project.WheneverAbstractions.StatusEffects;

namespace WheneverAbstractions._Project.WheneverAbstractions.Commands
{
    public class AddStatusEffectCommand: ITargetedWorldCommand
    {
        public CombatantId Target { get; }
        public StatusEffect statusEffect;
    
        public AddStatusEffectCommand(CombatantId target, StatusEffect effect)
        {
            Target = target;
            statusEffect = effect;
        }
    }
}