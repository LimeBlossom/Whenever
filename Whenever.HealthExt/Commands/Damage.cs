using Whenever.Core.WorldInterface;
using Whenever.HealthExt;

namespace Whenever.Core.Commands
{
    public record Damage : IGenericTargetedWorldCommand<ICommandWorldHealth>
    {
        public float damage;
        public CombatantId Target { get; }
        
        public Damage(CombatantId target, float damage)
        {
            this.damage = damage;
            Target = target;
        }
        
        public void ApplyCommand(ICommandWorldHealth world)
        {
            world.DoDamage(Target, damage);
        }
    }
}