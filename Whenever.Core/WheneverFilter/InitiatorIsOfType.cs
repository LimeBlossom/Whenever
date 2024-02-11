using Whenever.Core.CommandInitiators;
using Whenever.Core.WheneverTestDemo;
using Whenever.Core.WorldInterface;

namespace Whenever.Core.WheneverFilter
{
    public record InitiatorIsOfType : IWheneverFilter<IInspectableWorldDemo, ICommandableWorldDemo>
    {
        private readonly WheneverCombatantTypeFilter combatTypeFilter;

        public InitiatorIsOfType(WheneverCombatantTypeFilter combatTypeFilter)
        {
            this.combatTypeFilter = combatTypeFilter;
        }

        public bool TriggersOn(InitiatedCommand<ICommandableWorldDemo> initiatedCommand, IInspectableWorldDemo world)
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