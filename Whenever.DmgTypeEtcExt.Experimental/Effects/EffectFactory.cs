using System;
using Whenever.DmgTypeEtcExt.Experimental.World;
using static StandardAliases;

namespace Whenever.DmgTypeEtcExt.Experimental.Effects
{
    using EffectType = IEffect<IInspectableWorldDemo, ICommandableWorldDemo>;
    public static class EffectFactory
    {

        public static EffectType HealInitiator(float healAmount)
        {
            return Heal(Initiator, healAmount);
        }
        public static EffectType RandomBoulder(float meteorDamage)
        {
            return new RandomBoulderEffect(meteorDamage);
        }
        
        public static EffectType CriticalDamage(float critDamageMultiplier = 1)
        {
            return CritDamage(Target, critDamageMultiplier);
        }
        
        public static EffectType DamageTarget(DamageType damageType, float damageAmount)
        {
            return Damage(Target, damageType, damageAmount);
        }
        public static EffectType DamageInitiator(DamageType damageType, float damageAmount)
        {
            return Damage(Initiator, damageType, damageAmount);
        }

        public static EffectType DamageAdjacentTargets(DamageType type, float damageAmount)
        {
            return DamageAdjacentTo(Target, type, damageAmount);
        }
        
        public static EffectType Heal(CombatantAlias alias, float healAmount)
        {
            return new DamageCombatantEffect(alias)
            {
                damagePackage = new(DamageType.HEAL, -healAmount)
            };
        }
        
        public static EffectType Damage(CombatantAlias alias, DamageType type, float damageAmount)
        {
            return new DamageCombatantEffect(alias)
            {
                damagePackage = new(type, damageAmount)
            };
        }
        
        public static EffectType CritDamage(CombatantAlias alias, float critDamageMultiplier = 1)
        {
            return new ApplyCriticalDamageEffect(alias)
            {
                critDamageMultiplier = critDamageMultiplier
            };
        }
        
        public static EffectType DamageAdjacentTo(CombatantAlias alias, DamageType type, float damageAmount)
        {
            return new DamageAdjacentToCombatantEffect(alias)
            {
                damageType = type,
                damageAmount = damageAmount
            };
        }
    }
}