namespace WheneverAbstractions._Project.WheneverAbstractions.Effects
{
    public static class EffectFactory
    {
        public static IEffect Burn()
        {
            return new BurnEffect();
        }

        public static IEffect Heal(float healAmount)
        {
            return new HealEffect(healAmount);
        }
    }
}