using System;
using WheneverAbstractions._Project.WheneverAbstractions.Effects;
using WheneverAbstractions._Project.WheneverAbstractions.WheneverFilter;

namespace WheneverAbstractions._Project.WheneverAbstractions
{
    public class Whenever
    {
        private IWheneverFilter filter;
        private IEffect effect;

        public Whenever(IWheneverFilter filter, IEffect effect)
        {
            this.filter = filter;
            this.effect = effect;
        }

        public void TryTrigger(DamagePackage damagePackage, Combatant triggerTarget)
        {
            throw new NotImplementedException();
            // if (triggerTarget != damagePackage.target) return;
            //
            // var targetEnumType = triggerTarget.combatantType switch
            // {
            //     CombatantType.Player => Target.Player,
            //     CombatantType.Enemy => Target.Enemy,
            //     _ => throw new ArgumentOutOfRangeException()
            // };
            // if((target & targetEnumType) == 0) return;
            // if (damagePackage.damageType != trigger) return;
            //
            //     /*newDamagePackage =*/ effect?.Invoke(damagePackage, triggerTarget);
            // if success then WheneverManager.CheckWhenevers
        }
    }
}