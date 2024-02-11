using System.Collections.Generic;
using Whenever.Core.Commands;

namespace Whenever.Core.Effects
{
    public interface IEffect
    {
        public IEnumerable<IWorldCommand> ApplyEffect(InitiatedCommand command, IInspectableWorld world);
    }
}