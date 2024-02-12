namespace HealthFac
{
    public static class Commands
    {
        public static IWorldCommand<ICommandWorldHealth> Status(CombatantId target, StatusEffect<ICommandWorldHealth> effect)
        {
            return new AddStatusEffectCommand(target, effect);
        }
        
        public static IWorldCommand<ICommandWorldHealth> Damage(CombatantId target, float damage)
        {
            return new Damage(target, damage);
        }
    }
}