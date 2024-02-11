using Whenever.Core.WheneverTestDemo;
using Whenever.Core.WorldInterface;

namespace Whenever.Core.Commands
{
    public record DamageCommand : ITargetedWorldCommand
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
    }
}