using System.Collections.Generic;
using System.Linq;
using Whenever.Core.CommandInitiators;
using Whenever.Core.Effects;
using Whenever.Core.WheneverFilter;

namespace Whenever.Core
{
    public record Whenever
    {
        public IWheneverFilter filter;
        public IEffect effect;

        public Whenever(IWheneverFilter filter, IEffect effect)
        {
            this.filter = filter;
            this.effect = effect;
        }

        public IEnumerable<InitiatedCommand> GetTriggeredCommands(InitiatedCommand command, IInspectableWorld world)
        {
            if (!filter.TriggersOn(command, world)) return Enumerable.Empty<InitiatedCommand>();

            var nextInitiator = InitiatorFactory.FromEffectOf(command.initiator);
            
            return effect.ApplyEffect(command, world).Select(x => new InitiatedCommand(x, nextInitiator));
        }
    }
}