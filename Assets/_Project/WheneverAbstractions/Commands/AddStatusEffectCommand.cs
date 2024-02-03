public class AddStatusEffectCommand: IWorldCommand
{
    public CombatantId Target { get; }
    public StatusEffect statusEffect;
    
    public AddStatusEffectCommand(CombatantId target, StatusEffect effect)
    {
        Target = target;
        statusEffect = effect;
    }
}