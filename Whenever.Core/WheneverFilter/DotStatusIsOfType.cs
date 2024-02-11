using Whenever.Core.Commands;
using Whenever.Core.StatusEffects;

namespace Whenever.Core.WheneverFilter
{
    public record DotStatusIsOfType : IWheneverFilter
    {
        private readonly DamageType damageType;

        public DotStatusIsOfType(DamageType damageType)
        {
            this.damageType = damageType;
        }

        public bool TriggersOn(InitiatedCommand initiatedCommand, IInspectableWorld world)
        {
            if(initiatedCommand.command is AddStatusEffectCommand { statusEffect: DotStatus dotStatus })
            {
                return dotStatus.damagePackage.damageType == damageType;
            }

            return false;
        }
    }
}