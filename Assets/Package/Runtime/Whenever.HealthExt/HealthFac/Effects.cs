using System;
using HealthExtInternal;
using static StandardAliases;

namespace HealthFac
{
    internal class Effects
    {
        public static IEffect<IInspectWorldHealth, ICommandWorldHealth> DotTarget(float damage = 1, int turns = 3)
        {
            return Dot(Target, damage, turns);
        }
        
        public static IEffect<IInspectWorldHealth, ICommandWorldHealth> DamageTarget(float damage)
        {
            return Damage(Target, damage);
        }
        
        public static IEffect<IInspectWorldHealth, ICommandWorldHealth> DamageSpecificTarget(float damage, CombatantId specificTarget)
        {
            return new DamageSpecificEffect(specificTarget)
            {
                damage = damage
            };
        }
        
        public static IEffect<IInspectWorldHealth, ICommandWorldHealth> Dot(CombatantAlias alias, float damage = 1, int turns = 3)
        {
            return new DotCombatantEffect(alias)
            {
                damage = damage,
                turns = turns,
            };
        }
        
        public static IEffect<IInspectWorldHealth, ICommandWorldHealth> Damage(CombatantAlias alias, float damage)
        {
            return new DamageCombatantEffect(alias)
            {
                damage = damage
            };
        }
    }
}