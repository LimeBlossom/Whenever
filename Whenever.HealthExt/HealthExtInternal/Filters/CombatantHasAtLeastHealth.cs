using System;
using Serialization;
using UnityEngine;

namespace HealthExtInternal
{
    internal record CombatantHasAtLeastHealth : IWheneverFilter<IInspectWorldHealth, ICommandWorldHealth>
    {
        public readonly CombatantAlias alias;
        public float atLeast;

        public CombatantHasAtLeastHealth(CombatantAlias alias, float atLeast)
        {
            this.alias = alias;
            this.atLeast = atLeast;
        }

        public bool TriggersOn(
            InitiatedCommand<ICommandWorldHealth> initiatedCommand,
            IAliasCombatantIds aliaser,
            IInspectWorldHealth world)
        {
            var target = aliaser.GetIdForAlias(alias);
            if (target == null)
            {
                Debug.LogWarning($"Could not find target for alias '{alias}'");
                return false;
            }
            
            return world.GetHealth(target) >= atLeast;
        }

        public string Describe(IDescriptionContext context)
        {
            return $"{context.NameOf(alias)} has at least {atLeast} health";
        }
    }
}