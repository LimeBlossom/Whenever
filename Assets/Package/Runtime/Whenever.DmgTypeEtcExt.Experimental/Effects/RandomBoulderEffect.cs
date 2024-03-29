using System.Collections.Generic;
using System.Linq;
using Whenever.DmgTypeEtcExt.Experimental.Commands;
using Whenever.DmgTypeEtcExt.Experimental.World;

namespace Whenever.DmgTypeEtcExt.Experimental.Effects
{
    public record RandomBoulderEffect: IEffect<IInspectableWorldDemo, ICommandableWorldDemo>
    {
        private float meteorDamage;

        public RandomBoulderEffect(float meteorDamage)
        {
            this.meteorDamage = meteorDamage;
        }

        public IEnumerable<IWorldCommand<ICommandableWorldDemo>> ApplyEffect(
            InitiatedCommand<ICommandableWorldDemo> command,
            IAliasCombatantIds aliaser,
            IInspectableWorldDemo world)
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

        public string Describe(IDescriptionContext context)
        {
            return $"deal {meteorDamage} {DamageType.PHYSICAL} damage to a random target";
        }
    }
}