using Whenever.Core.Commands;
using Whenever.Core.WheneverTestDemo;
using Whenever.Core.WorldInterface;

namespace Whenever.Core.WheneverFilter
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
            if (initiatedCommand.command is not ITargetedWorldCommand targetedCommand) return false;
            
            var combatantType = world.CombatantData(targetedCommand.Target).GetCombatantType();
                

            var targetEnumType = combatantType.ToTypeFilter();
            return (combatTypeFilter & targetEnumType) != 0;
        }
    }
}