﻿public record AddStatusEffectCommand<TCommand>: IGenericTargetedWorldCommand<TCommand>
    where TCommand : ICommandWorld, ICommandWorldStatusEffects<TCommand>
{
    public CombatantId Target { get; }
        
    public StatusEffect<TCommand> statusEffect;
    
    public AddStatusEffectCommand(CombatantId target, StatusEffect<TCommand> effect)
    {
        Target = target;
        statusEffect = effect;
    }

    public void ApplyCommand(TCommand world)
    {
        world.AddStatusEffect(Target, statusEffect);
    }

    public string Describe(IDescribeCombatants context)
    {
        return $"add {statusEffect.Describe(context)} to {context.NameOf(Target)}";
    }
}