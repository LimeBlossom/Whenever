using Whenever.DmgTypeEtcExt.Experimental.World;

namespace Whenever.DmgTypeEtcExt.Experimental.Filters
{
    public record TargetIsOfType : IWheneverFilter<IInspectableWorldDemo, ICommandableWorldDemo>
    {
        private readonly WheneverCombatantTypeFilter combatTypeFilter;

        public TargetIsOfType(WheneverCombatantTypeFilter combatTypeFilter)
        {
            this.combatTypeFilter = combatTypeFilter;
        }

        public bool TriggersOn(InitiatedCommand<ICommandableWorldDemo> initiatedCommand, IInspectableWorldDemo world)
        {
            if (initiatedCommand.command is not IGenericTargetedWorldCommand<ICommandableWorldDemo> targetedCommand) return false;
            
            var combatantType = world.CombatantData(targetedCommand.Target).GetCombatantType();
                

            var targetEnumType = combatantType.ToTypeFilter();
            return (combatTypeFilter & targetEnumType) != 0;
        }
        
        public string Describe(IDescriptionContext context)
        {
            return $"{context.TargetName} is {combatTypeFilter}";
        }
    }
}