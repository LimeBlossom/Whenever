using Whenever.Core;
using Whenever.Core.Commands;
using Whenever.HealthExt.World;

namespace Whenever.HealthExt.Commands
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