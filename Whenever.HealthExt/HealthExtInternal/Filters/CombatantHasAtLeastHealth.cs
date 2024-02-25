using System;
using Serialization;
using UnityEngine;

namespace HealthExtInternal
{
    [PolymorphicSerializable("CombatantHasAtLeastHealth"), Serializable]
    internal record CombatantHasAtLeastHealth : IWheneverFilter<IInspectWorldHealth, ICommandWorldHealth>
    {
        public CombatantAlias combatant;
        public float atLeast;

        public CombatantHasAtLeastHealth(CombatantAlias combatant, float atLeast)
        {
            this.combatant = combatant;
            this.atLeast = atLeast;
        }

        public bool TriggersOn(
            InitiatedCommand<ICommandWorldHealth> initiatedCommand,
            IAliasCombatantIds aliaser,
            IInspectWorldHealth world)
        {
            var target = aliaser.GetIdForAlias(combatant);
            if (target == null)
            {
                Debug.LogWarning($"Could not find target for alias '{combatant}'");
                return false;
            }
            
            return world.GetHealth(target) >= atLeast;
        }

        public string Describe(IDescriptionContext context)
        {
            return $"{context.NameOf(combatant)} has at least {atLeast} health";
        }
    }
}