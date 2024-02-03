namespace _Project.WheneverAbstractions
{
    public static class CommandFactory
    {
        public static IWorldCommand Damage(DamageType type, int amount, CombatantId target)
        { 
            return new DamageCommand()
            {
                damagePackage = new(type, amount),
                Target = target
            };
        }
        
        public static IWorldCommand AddStatusEffect(CombatantId target, StatusEffect effect)
        {
            return new AddStatusEffectCommand(target, effect);
        }
        
        public static IWorldCommand AddWheneverEffect(CombatantId target, Whenever whenever)
        {
            return new AddWheneverCommand(target, whenever);
        }
        
    }
}