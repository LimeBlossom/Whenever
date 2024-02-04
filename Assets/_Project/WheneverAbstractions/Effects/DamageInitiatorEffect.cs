using System.Collections.Generic;
using WheneverAbstractions._Project.WheneverAbstractions.Commands;

namespace WheneverAbstractions._Project.WheneverAbstractions.Effects
{
    public record DamageInitiatorEffect: EffectInitiatorEffect
    {
        public DamagePackage damagePackage;
        protected override IEnumerable<IWorldCommand> ApplyEffectToInitiator(CombatantId initiator, IInspectableWorld world)
        {
            yield return new DamageCommand(initiator, damagePackage);
        }
    }
}