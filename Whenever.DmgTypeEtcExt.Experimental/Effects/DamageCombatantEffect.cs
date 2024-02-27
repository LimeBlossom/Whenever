using System.Collections.Generic;
using UnityEngine;
using Whenever.DmgTypeEtcExt.Experimental.Commands;
using Whenever.DmgTypeEtcExt.Experimental.World;

namespace Whenever.DmgTypeEtcExt.Experimental.Effects
{
    public record DamageCombatantEffect: EffectAliasedTargetEffect<IInspectableWorldDemo, ICommandableWorldDemo>
    {
        public DamagePackage damagePackage;
        
        public DamageCombatantEffect(CombatantAlias alias) : base(alias)
        {
        }
        protected override IEnumerable<IWorldCommand<ICommandableWorldDemo>> ApplyEffectToTarget(
            CombatantId target,
            InitiatedCommand<ICommandableWorldDemo> command,
            IInspectableWorldDemo world)
        {
            yield return new DamageCommand(target, damagePackage);
        }

        protected override string DescribeOnTarget()
        {
            return $"deal {damagePackage.damageAmount} {damagePackage.damageType} damage";
        }
    }
}