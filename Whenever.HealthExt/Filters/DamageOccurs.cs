using Whenever.Core.Commands;
using Whenever.Core.WheneverFilter;
using Whenever.HealthExt.Commands;
using Whenever.HealthExt.World;

namespace Whenever.HealthExt.Filters
{
    public record DamageOccurs : IWheneverFilter<IInspectWorldHealth, ICommandWorldHealth>
    {
        private readonly float atLeast;

        public DamageOccurs(float atLeast)
        {
            this.atLeast = atLeast;
        }

        public bool TriggersOn(InitiatedCommand<ICommandWorldHealth> initiatedCommand, IInspectWorldHealth world)
        {
            if (initiatedCommand.command is Damage damageCommand)
            {
                return damageCommand.damage >= atLeast;
            }
            
            return false;
        }
    }
}