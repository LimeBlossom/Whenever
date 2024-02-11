using System.Collections.Generic;
using Whenever.Core.Effects;
using Whenever.Core.WorldInterface;
using Whenever.DmgTypeEtcExt.Experimental.Commands;
using Whenever.DmgTypeEtcExt.Experimental.World;

namespace Whenever.DmgTypeEtcExt.Experimental.Effects
{
    public record DamageInitiatorEffect: EffectInitiatorEffect<IInspectableWorldDemo, ICommandableWorldDemo>
    {
        public DamagePackage damagePackage;
        protected override IEnumerable<IWorldCommand<ICommandableWorldDemo>> ApplyEffectToInitiator(CombatantId initiator, IInspectableWorldDemo world)
        {
            yield return new DamageCommand(initiator, damagePackage);
        }
    }
}