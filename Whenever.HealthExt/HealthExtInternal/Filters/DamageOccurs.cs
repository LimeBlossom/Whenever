using System;
using Serialization;
using UnityEngine;

namespace HealthExtInternal
{
    [PolymorphicSerializable("DamageOccurs"), Serializable]
    internal record DamageOccurs : IWheneverFilter<IInspectWorldHealth, ICommandWorldHealth>
    {
        [SerializeField]
        private float atLeast;

        public DamageOccurs(float atLeast)
        {
            this.atLeast = atLeast;
        }

        public bool TriggersOn(InitiatedCommand<ICommandWorldHealth> initiatedCommand, IInspectWorldHealth world)
        {
            if (initiatedCommand.command is Damage damageCommand)
            {
                return damageCommand.damage >= atLeast;
            }
            
            return false;
        }

        public string Describe()
        {
            return $"at least {atLeast} damage occurs";
        }
    }
}