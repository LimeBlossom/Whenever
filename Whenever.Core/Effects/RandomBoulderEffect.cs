using System.Collections.Generic;
using System.Linq;
using Whenever.Core.Commands;
using Whenever.Core.WheneverTestDemo;
using Whenever.Core.WorldInterface;

namespace Whenever.Core.Effects
{
    public record RandomBoulderEffect: IEffect<IInspectableWorldDemo, ICommandableWorldDemo>
    {
        private float meteorDamage;

        public RandomBoulderEffect(float meteorDamage)
        {
            this.meteorDamage = meteorDamage;
        }

        public IEnumerable<IWorldCommand<ICommandableWorldDemo>> ApplyEffect(InitiatedCommand<ICommandableWorldDemo> command, IInspectableWorldDemo world)
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