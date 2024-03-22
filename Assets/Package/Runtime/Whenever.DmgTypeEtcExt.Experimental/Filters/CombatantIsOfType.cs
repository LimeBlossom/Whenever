using System;
using UnityEngine;
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
            var target = aliaser.GetIdForAlias(alias);
            if (target == null)
            {
                Debug.LogWarning($"Could not find target for alias '{alias}'");
                return false;
            }
            
            var combatantType = world.CombatantData(target).GetCombatantType();

            var targetEnumType = combatantType.ToTypeFilter();
            return (combatTypeFilter & targetEnumType) != 0;
        }
        
        public string Describe(IDescriptionContext context)
        {
            return $"{context.NameOf(alias)} is {combatTypeFilter}";
        }
    }
}