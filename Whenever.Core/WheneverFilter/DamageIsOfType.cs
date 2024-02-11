using Whenever.Core.Commands;

namespace Whenever.Core.WheneverFilter
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