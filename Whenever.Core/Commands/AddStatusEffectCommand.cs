﻿using Whenever.Core.StatusEffects;

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

        public void ApplyCommand(ICommandableWorld world)
        {
            var target = world.GetCombatantRaw(Target);
            target.statusEffects.Add(statusEffect);
        }
    }
}