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

    public string Describe()
    {
        return $"add status effect {statusEffect} to {Target}";
    }
}