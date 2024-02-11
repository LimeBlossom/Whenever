namespace WheneverAbstractions._Project.WheneverAbstractions.Effects
{
    public static class EffectFactory
    {
        public static IEffect BurnTarget(float damage = 1, int turns = 3)
        {
            return new DotStatusTargetEffect
            {
                damagePackage = new (DamageType.BURN,  damage),
                turns = turns
            };
        }
        public static IEffect BleedTarget(float bleedDamage, int turns)
        {
            return new DotStatusTargetEffect
            {
                damagePackage = new (DamageType.BLEED, bleedDamage),
                turns = turns
            };
        }

        public static IEffect HealInitiator(float healAmount)
        {
            return new HealInitiatorEffect(healAmount);
        }
        public static IEffect RandomBoulder(float meteorDamage)
        {
            return new RandomBoulderEffect(meteorDamage);
        }
        
        public static IEffect CriticalDamage(float critDamageMultiplier = 1)
        {
            return new ApplyCriticalDamageEffect
            {
                critDamageMultiplier = critDamageMultiplier
            };
        }
        
        public static IEffect DamageTarget(DamageType damageType, float damageAmount)
        {
            return new DamageTargetEffect
            {
                damagePackage = new(damageType, damageAmount)
            };
        }
        public static IEffect DamageInitiator(DamageType damageType, float damageAmount)
        {
            return new DamageInitiatorEffect
            {
                damagePackage = new(damageType, damageAmount)
            };
        }

        public static IEffect DamageAdjacentTargets(DamageType type, float damageAmount)
        {
            return new DamageAdjacentToTargetEffect
            {
                damageAmount = damageAmount,
                damageType = type
            };
        }
    }
}