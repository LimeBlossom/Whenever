using WheneverAbstractions._Project.WheneverAbstractions.Commands;

namespace WheneverAbstractions._Project.WheneverAbstractions.WheneverFilter
{
    public record DamageIsOfType : IWheneverFilter
    {
        public readonly DamageType damageType;

        public DamageIsOfType(DamageType damageType)
        {
            this.damageType = damageType;
        }

        public bool TriggersOn(InitiatedCommand initiatedCommand, IInspectableWorld world)
        {
            if(initiatedCommand.command is DamageCommand damageCommand)
            {
                return damageCommand.damagePackage.damageType == damageType;
            }

            return false;
        }
    }
}