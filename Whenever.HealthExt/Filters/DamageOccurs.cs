using Whenever.Core.WorldInterface;

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
            if (initiatedCommand.command is Core.Commands.Damage damageCommand)
            {
                return damageCommand.damage >= atLeast;
            }
            
            return false;
        }
    }
}