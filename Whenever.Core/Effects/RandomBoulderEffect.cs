using System.Collections.Generic;
using System.Linq;
using WheneverAbstractions._Project.WheneverAbstractions.Commands;

namespace WheneverAbstractions._Project.WheneverAbstractions.Effects
{
    public record RandomBoulderEffect: IEffect
    {
        private float meteorDamage;

        public RandomBoulderEffect(float meteorDamage)
        {
            this.meteorDamage = meteorDamage;
        }

        public IEnumerable<IWorldCommand> ApplyEffect(InitiatedCommand command, IInspectableWorld world)
        {
            var allCombatants = world.AllIds().ToArray();
            var randomSelection = world.GetRng().Next(0, allCombatants.Length);
            
            var target = allCombatants[randomSelection];
            var damagePackage = new DamagePackage
            {
                damageAmount = meteorDamage,
                damageType = DamageType.PHYSICAL
            };
            yield return new DamageCommand(target, damagePackage);
        }
    }
}