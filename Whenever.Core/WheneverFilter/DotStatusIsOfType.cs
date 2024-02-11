using Whenever.Core.Commands;
using Whenever.Core.StatusEffects;
using Whenever.Core.WheneverTestDemo;
using Whenever.Core.WorldInterface;

namespace Whenever.Core.WheneverFilter
{
    public record DotStatusIsOfType : IWheneverFilter<IInspectableWorldDemo, ICommandableWorldDemo>
    {
        private readonly DamageType damageType;

        public DotStatusIsOfType(DamageType damageType)
        {
            this.damageType = damageType;
        }

        public bool TriggersOn(InitiatedCommand<ICommandableWorldDemo> initiatedCommand, IInspectableWorldDemo world)
        {
            if(initiatedCommand.command is AddStatusEffectCommand { statusEffect: DotStatus dotStatus })
            {
                return dotStatus.damagePackage.damageType == damageType;
            }

            return false;
        }
    }
}