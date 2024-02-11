using System.Collections.Generic;
using Whenever.Core.Commands;
using Whenever.Core.WorldInterface;

namespace Whenever.Core.Effects
{
    public interface IEffect<in TInspectWorld, TCommandWorld>
        where TInspectWorld : IInspectWorld
        where TCommandWorld : ICommandWorld
    {
        public IEnumerable<IWorldCommand<TCommandWorld>> ApplyEffect(
            InitiatedCommand<TCommandWorld> command,
            TInspectWorld world);
    }
}