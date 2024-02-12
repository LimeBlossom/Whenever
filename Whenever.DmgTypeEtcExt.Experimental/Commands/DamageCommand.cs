using Whenever.Core;
using Whenever.Core.Commands;
using Whenever.Core.WorldInterface;
using Whenever.DmgTypeEtcExt.Experimental.World;

namespace Whenever.DmgTypeEtcExt.Experimental.Commands
{
    public record DamageCommand : IGenericTargetedWorldCommand<ICommandableWorldDemo>
    {
        public DamagePackage damagePackage;
        public CombatantId Target { get; }
        
        public DamageCommand(CombatantId target, DamagePackage damagePackage)
        {
            this.damagePackage = damagePackage;
            Target = target;
        }
        
        public void ApplyCommand(ICommandableWorldDemo world)
        {
            var target = world.GetCombatantRaw(Target);
            var withResistance = target.damageable.ApplyResistances(damagePackage);
            target.health.Change(withResistance.damageAmount);
        }

        public string Describe()
        {
            return $"deal {damagePackage.damageAmount} {damagePackage.damageType} to {Target}";
        }
    }
}