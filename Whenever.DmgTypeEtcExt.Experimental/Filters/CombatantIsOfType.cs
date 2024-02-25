using System;
using Whenever.DmgTypeEtcExt.Experimental.World;

namespace Whenever.DmgTypeEtcExt.Experimental.Filters
{
    public record CombatantIsOfType : IWheneverFilter<IInspectableWorldDemo, ICommandableWorldDemo>
    {
        private readonly CombatantAlias alias;
        private readonly WheneverCombatantTypeFilter combatTypeFilter;

        public CombatantIsOfType(CombatantAlias alias, WheneverCombatantTypeFilter combatTypeFilter)
        {
            this.alias = alias;
            this.combatTypeFilter = combatTypeFilter;
        }

        public bool TriggersOn(
            InitiatedCommand<ICommandableWorldDemo> initiatedCommand,
            IAliasCombatantIds aliaser,
            IInspectableWorldDemo world)
        {
            if (initiatedCommand.command is not IGenericTargetedWorldCommand<ICommandableWorldDemo> targetedCommand) return false;
            throw new NotImplementedException();
            
            var combatantType = world.CombatantData(targetedCommand.Target).GetCombatantType();
                

            var targetEnumType = combatantType.ToTypeFilter();
            return (combatTypeFilter & targetEnumType) != 0;
        }
        
        public string Describe(IDescriptionContext context)
        {
            return $"{context.NameOf(alias)} is {combatTypeFilter}";
        }
    }
}