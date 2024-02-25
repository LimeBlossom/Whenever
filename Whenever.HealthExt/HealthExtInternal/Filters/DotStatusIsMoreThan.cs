using System;
using Serialization;
using UnityEngine;

namespace HealthExtInternal
{
    [PolymorphicSerializable("DotStatusIsMoreThan"), Serializable]
    internal record DotStatusIsMoreThan : IWheneverFilter<IInspectWorldHealth, ICommandWorldHealth>
    {
        [SerializeField]
        private float atLeast;

        public DotStatusIsMoreThan(float atLeast)
        {
            this.atLeast = atLeast;
        }

        public bool TriggersOn(InitiatedCommand<ICommandWorldHealth> initiatedCommand, IInspectWorldHealth world)
        {
            if(initiatedCommand.command is AddStatusEffectCommand<ICommandWorldHealth> { statusEffect: DotStatus dotStatus })
            {
                return dotStatus.damage >= atLeast;
            }

            return false;
        }

        public string Describe(IDescriptionContext context)
        {
            return $"a status effect of at least {atLeast} damage per turn is inflicted";
        }
    }
}