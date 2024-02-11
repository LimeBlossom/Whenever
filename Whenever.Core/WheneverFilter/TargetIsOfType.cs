using WheneverAbstractions._Project.WheneverAbstractions.Commands;

namespace WheneverAbstractions._Project.WheneverAbstractions.WheneverFilter
{
    public record TargetIsOfType : IWheneverFilter
    {
        private readonly WheneverCombatantTypeFilter combatTypeFilter;

        public TargetIsOfType(WheneverCombatantTypeFilter combatTypeFilter)
        {
            this.combatTypeFilter = combatTypeFilter;
        }

        public bool TriggersOn(InitiatedCommand initiatedCommand, IInspectableWorld world)
        {
            if (initiatedCommand.command is not ITargetedWorldCommand targetedCommand) return false;
            
            var combatantType = world.CombatantData(targetedCommand.Target).GetCombatantType();
                

            var targetEnumType = combatantType.ToTypeFilter();
            return (combatTypeFilter & targetEnumType) != 0;
        }
    }
}