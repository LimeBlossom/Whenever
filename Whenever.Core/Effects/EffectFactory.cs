﻿using Whenever.Core.WheneverTestDemo;
using Whenever.Core.WorldInterface;

namespace Whenever.Core.Effects
{
    public static class EffectFactory
    {

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

        public static IEffect<IInspectableWorldDemo, ICommandableWorldDemo> BurnTarget()
        {
            throw new System.NotImplementedException();
        }

        public static IEffect<IInspectableWorldDemo, ICommandableWorldDemo> BleedTarget(int i, int i1)
        {
            throw new System.NotImplementedException();
        }
    }
}