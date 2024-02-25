using System;
using Serialization;
using UnityEngine;

namespace HealthExtInternal
{
    [PolymorphicSerializable("TargetHasAtLeastHealth"), Serializable]
    internal record TargetHasAtLeastHealth : IWheneverFilter<IInspectWorldHealth, ICommandWorldHealth>
    {
        public float atLeast;

        public TargetHasAtLeastHealth(float atLeast)
        {
            this.atLeast = atLeast;
        }

        public bool TriggersOn(
            InitiatedCommand<ICommandWorldHealth> initiatedCommand,
            IAliasCombatantIds aliaser,
            IInspectWorldHealth world)
        {
            if (initiatedCommand.command is IGenericTargetedWorldCommand<ICommandWorldHealth> targetedCommand)
            {
                return world.GetHealth(targetedCommand.Target) >= atLeast;
            }
            
            return false;
        }

        public string Describe(IDescriptionContext context)
        {
            return $"{context.TargetName} has at least {atLeast} health";
        }
    }
}