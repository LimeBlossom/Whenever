using System.Collections.Generic;
using Whenever.Core.Effects;
using Whenever.Core.WorldInterface;
using Whenever.DmgTypeEtcExt.Experimental.Commands;
using Whenever.DmgTypeEtcExt.Experimental.World;

namespace Whenever.DmgTypeEtcExt.Experimental.Effects
{
    public record DamageTargetEffect: EffectTargetEffect<IInspectableWorldDemo, ICommandableWorldDemo>
    {
        public DamagePackage damagePackage;

        protected override IEnumerable<IWorldCommand<ICommandableWorldDemo>> ApplyEffectToTarget(CombatantId target, IInspectableWorldDemo world)
        {
            yield return new DamageCommand(target, damagePackage);
        }
    }
}