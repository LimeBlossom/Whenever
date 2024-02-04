using System;
using WheneverAbstractions._Project.WheneverAbstractions.StatusEffects;

namespace WheneverAbstractions._Project.WheneverAbstractions.Effects
{
    public static class EffectFactory
    {
        public static IEffect BurnTarget()
        {
            return new BurnTargetEffect();
        }
        public static IEffect BleedTarget(float bleedDamage, int turns)
        {
            return new BleedTargetEffect
            {
                bleedDamage = bleedDamage,
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
        
    }
}