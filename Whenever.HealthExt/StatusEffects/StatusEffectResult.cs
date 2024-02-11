using System.Collections.Generic;
using Whenever.Core.WorldInterface;

namespace Whenever.HealthExt.StatusEffects
{
    public struct StatusEffectResult<TCommand>
        where TCommand: ICommandWorld
    {
        public StatusEffectCompletion completion;
        public IEnumerable<IWorldCommand<TCommand>> commands;
    }
}