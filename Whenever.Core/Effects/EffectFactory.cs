using Whenever.Core.WheneverTestDemo;
using Whenever.Core.WorldInterface;

namespace Whenever.Core.Effects
{
    public static class EffectFactory
    {
        public static IEffect<IInspectableWorldDemo, ICommandableWorldDemo> BurnTarget(float damage = 1, int turns = 3)
        {
            return new DotStatusTargetEffect
            {
                damagePackage = new (DamageType.BURN,  damage),
                turns = turns
            };
        }
        public static IEffect<IInspectableWorldDemo, ICommandableWorldDemo> BleedTarget(float bleedDamage, int turns)
        {
            return new DotStatusTargetEffect
            {
                damagePackage = new (DamageType.BLEED, bleedDamage),
                turns = turns
            };
        }

        public static IEffect<IInspectableWorldDemo, ICommandableWorldDemo> HealInitiator(float healAmount)
        {
            return new HealInitiatorEffect(healAmount);
        }
        public static IEffect<IInspectableWorldDemo, ICommandableWorldDemo> RandomBoulder(float meteorDamage)
        {
            return new RandomBoulderEffect(meteorDamage);
        }
        
        public static IEffect<IInspectableWorldDemo, ICommandableWorldDemo> CriticalDamage(float critDamageMultiplier = 1)
        {
            return new ApplyCriticalDamageEffect
            {
                critDamageMultiplier = critDamageMultiplier
            };
        }
        
        public static IEffect<IInspectableWorldDemo, ICommandableWorldDemo> DamageTarget(DamageType damageType, float damageAmount)
        {
            return new DamageTargetEffect
            {
                damagePackage = new(damageType, damageAmount)
            };
        }
        public static IEffect<IInspectableWorldDemo, ICommandableWorldDemo> DamageInitiator(DamageType damageType, float damageAmount)
        {
            return new DamageInitiatorEffect
            {
                damagePackage = new(damageType, damageAmount)
            };
        }

        public static IEffect<IInspectableWorldDemo, ICommandableWorldDemo> DamageAdjacentTargets(DamageType type, float damageAmount)
        {
            return new DamageAdjacentToTargetEffect
            {
                damageAmount = damageAmount,
                damageType = type
            };
        }
    }
}