using Whenever.Core.Commands;
using Whenever.Core.StatusEffects;
using Whenever.Core.WorldInterface;

namespace Whenever.HealthExt.Filters
{
    public record DotStatusIsMoreThan : IWheneverFilter<IInspectWorldHealth, ICommandWorldHealth>
    {
        private readonly float atLeast;

        public DotStatusIsMoreThan(float atLeast)
        {
            this.atLeast = atLeast;
        }

        public bool TriggersOn(InitiatedCommand<ICommandWorldHealth> initiatedCommand, IInspectWorldHealth world)
        {
            if(initiatedCommand.command is AddStatusEffectCommand { statusEffect: DotStatus dotStatus })
            {
                return dotStatus.damage >= atLeast;
            }

            return false;
        }
    }
}