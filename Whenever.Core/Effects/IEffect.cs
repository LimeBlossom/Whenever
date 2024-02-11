using System.Collections.Generic;

namespace Whenever.Core.WorldInterface
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