using Whenever.Core.Commands;
using Whenever.Core.WheneverFilter;
using Whenever.Core.WorldInterface;
using Whenever.DmgTypeEtcExt.Experimental.Commands;
using Whenever.DmgTypeEtcExt.Experimental.World;

namespace Whenever.DmgTypeEtcExt.Experimental.Filters
{
    public record DamageIsOfType : IWheneverFilter<IInspectableWorldDemo, ICommandableWorldDemo>
    {
        public readonly DamageType damageType;

        public DamageIsOfType(DamageType damageType)
        {
            this.damageType = damageType;
        }

        public bool TriggersOn(InitiatedCommand<ICommandableWorldDemo> initiatedCommand, IInspectableWorldDemo world)
        {
            if(initiatedCommand.command is DamageCommand damageCommand)
            {
                return damageCommand.damagePackage.damageType == damageType;
            }

            return false;
        }
    }
}