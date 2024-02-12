using Whenever.Core.Commands;
using Whenever.Core.WheneverFilter;
using Whenever.HealthExt.Commands;
using Whenever.HealthExt.World;

namespace Whenever.HealthExt.Filters
{
    public record TargetHasAtLeastHealth : IWheneverFilter<IInspectWorldHealth, ICommandWorldHealth>
    {
        private readonly float atLeast;

        public TargetHasAtLeastHealth(float atLeast)
        {
            this.atLeast = atLeast;
        }

        public bool TriggersOn(InitiatedCommand<ICommandWorldHealth> initiatedCommand, IInspectWorldHealth world)
        {
            if (initiatedCommand.command is IGenericTargetedWorldCommand<ICommandWorldHealth> targetedCommand)
            {
                return world.GetHealth(targetedCommand.Target) >= atLeast;
            }
            
            return false;
        }

        public string Describe()
        {
            return $"target has at least {atLeast} health";
        }
    }
}