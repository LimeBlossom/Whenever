using System;
using Serialization;
using UnityEngine;

namespace HealthExtInternal
{
    [PolymorphicSerializable("CombatantHasAtLeastHealth"), Serializable]
    internal record CombatantHasAtLeastHealth : IWheneverFilter<IInspectWorldHealth, ICommandWorldHealth>
    {
        public readonly CombatantAlias alias;
        public float atLeast;

        public CombatantHasAtLeastHealth(CombatantAlias alias, float atLeast)
        {
            this.alias = alias;
            this.atLeast = atLeast;
        }

        public bool TriggersOn(InitiatedCommand<ICommandWorldHealth> initiatedCommand, IInspectWorldHealth world)
        {
            if (initiatedCommand.command is IGenericTargetedWorldCommand<ICommandWorldHealth> targetedCommand)
            {
                throw new NotImplementedException();
                return world.GetHealth(targetedCommand.Target) >= atLeast;
            }
            
            return false;
        }

        public string Describe(IDescriptionContext context)
        {
            return $"{context.NameOf(alias)} has at least {atLeast} health";
        }
    }
}