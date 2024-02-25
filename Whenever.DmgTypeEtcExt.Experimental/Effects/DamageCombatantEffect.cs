using System.Collections.Generic;
using UnityEngine;
using Whenever.DmgTypeEtcExt.Experimental.Commands;
using Whenever.DmgTypeEtcExt.Experimental.World;

namespace Whenever.DmgTypeEtcExt.Experimental.Effects
{
    public record DamageCombatantEffect: IEffect<IInspectableWorldDemo, ICommandableWorldDemo>
    {
        public DamagePackage damagePackage;
        private readonly CombatantAlias alias;
        
        public DamageCombatantEffect(CombatantAlias alias)
        {
            this.alias = alias;
        }
        public IEnumerable<IWorldCommand<ICommandableWorldDemo>> ApplyEffect(
            InitiatedCommand<ICommandableWorldDemo> command,
            IAliasCombatantIds aliaser,
            IInspectableWorldDemo world)
        {
            var target = aliaser.GetIdForAlias(alias);
            if (target == null)
            {
                Debug.LogWarning($"Could not find target for alias '{alias}'");
                yield break;
            }

            yield return new DamageCommand(target, damagePackage);
        }

        public string Describe(IDescriptionContext context)
        {
            return $"deal {damagePackage.damageAmount} {damagePackage.damageType} damage{context.ToAliasAsDirectSubject(alias)}";
        }
    }
}