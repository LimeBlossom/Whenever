using System.Collections.Generic;
using UnityEngine;
using Whenever.Core.Commands;
using Whenever.Core.StatusEffects;
using Whenever.Core.WheneverTestDemo;
using Whenever.Core.WorldInterface;

namespace Whenever.Core.Effects
{
    public record DotStatusTargetEffect: IEffect<IInspectableWorldDemo, ICommandableWorldDemo>
    {
        public DamagePackage damagePackage;
        public int turns;
        public IEnumerable<IWorldCommand<ICommandableWorldDemo>> ApplyEffect(InitiatedCommand<ICommandableWorldDemo> command, IInspectableWorldDemo world)
        {
            if (command.command is not ITargetedWorldCommand targetedCommand)
            {
                Debug.LogWarning("Burn Target effect can only apply on commands that target at least one combatant");
                yield break;
            }

            var status = new DotStatus(turns, command.initiator)
            {
                damagePackage = damagePackage
            };
            yield return new AddStatusEffectCommand(targetedCommand.Target, status);
        }
    }
}