using WheneverAbstractions._Project.WheneverAbstractions.Commands;
using WheneverAbstractions._Project.WheneverAbstractions.StatusEffects;

namespace WheneverAbstractions._Project.WheneverAbstractions.WheneverFilter
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