using Whenever.Core.Commands;
using Whenever.Core.WheneverTestDemo;
using Whenever.Core.WorldInterface;

namespace Whenever.Core.WheneverFilter
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