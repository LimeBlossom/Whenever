namespace WheneverAbstractions._Project.WheneverAbstractions.Effects
{
    public static class EffectFactory
    {
        public static IEffect BurnTarget()
        {
            return new BurnTargetEffect();
        }

        public static IEffect HealInitiator(float healAmount)
        {
            return new HealInitiatorEffect(healAmount);
        }
        public static IEffect RandomBoulder(float meteorDamage)
        {
            return new RandomBoulderEffect(meteorDamage);
        }
    }
}