using Whenever.Core.CommandInitiators;

namespace Whenever.Core.WheneverFilter
{
    public record InitiatorIsOfType : IWheneverFilter
    {
        private readonly WheneverCombatantTypeFilter combatTypeFilter;

        public InitiatorIsOfType(WheneverCombatantTypeFilter combatTypeFilter)
        {
            this.combatTypeFilter = combatTypeFilter;
        }

        public bool TriggersOn(InitiatedCommand initiatedCommand, IInspectableWorld world)
        {
            if (!initiatedCommand.initiator.TryAsOrRecursedFrom<CombatantCommandInitiator>(out var initiator))
            {
                return false;
            }
            
            var combatantType = world.CombatantData(initiator.Initiator).GetCombatantType();

            var targetEnumType = combatantType.ToTypeFilter();
            return (combatTypeFilter & targetEnumType) != 0;
        }
    }
}