﻿using WheneverAbstractions._Project.WheneverAbstractions.StatusEffects;

namespace WheneverAbstractions._Project.WheneverAbstractions.Commands
{
    public static class CmdFactory
    {
        public static IWorldCommand Damage(DamageType type, int amount, CombatantId target)
        { 
            return new DamageCommand(target, new DamagePackage(type, amount));
        }
        
        public static IWorldCommand Status(CombatantId target, StatusEffect effect)
        {
            return new AddStatusEffectCommand(target, effect);
        }
        
        public static IWorldCommand Whenever(Whenever whenever)
        {
            return new AddWheneverCommand(whenever);
        }
        
    }
}