using System.Collections.Generic;
using Whenever.Core.Commands;
using Whenever.Core.Effects;
using Whenever.Core.WorldInterface;
using Whenever.HealthExt.Commands;
using Whenever.HealthExt.World;

namespace Whenever.HealthExt.Effects
{
    public class DamageTargetEffect : IEffect<IInspectWorldHealth,ICommandWorldHealth>
    {
        public float damage;
        
        public IEnumerable<IWorldCommand<ICommandWorldHealth>> ApplyEffect(InitiatedCommand<ICommandWorldHealth> command, IInspectWorldHealth world)
        {
            if (command.command is not IGenericTargetedWorldCommand<ICommandWorldHealth> targetedCommand)
            {
                yield break;
            }
            yield return new Damage(targetedCommand.Target, 1);
        }
    }
}