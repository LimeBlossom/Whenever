using System.Collections.Generic;
using Whenever.Core.Commands;
using Whenever.Core.WheneverTestDemo;
using Whenever.Core.WorldInterface;

namespace Whenever.Core.Effects
{
    public record DamageInitiatorEffect: EffectInitiatorEffect
    {
        public DamagePackage damagePackage;
        protected override IEnumerable<IWorldCommand<ICommandableWorldDemo>> ApplyEffectToInitiator(CombatantId initiator, IInspectableWorldDemo world)
        {
            yield return new DamageCommand(initiator, damagePackage);
        }
    }
}