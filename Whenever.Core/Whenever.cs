using System.Collections.Generic;
using System.Linq;
using Whenever.Core.CommandInitiators;
using Whenever.Core.WorldInterface;

namespace Whenever.Core
{
    public record Whenever<TInspectWorld, TCommandWorld>
        where TInspectWorld : IInspectWorld
        where TCommandWorld : ICommandWorld
    {
        public IWheneverFilter<TInspectWorld, TCommandWorld> filter;
        public IEffect<TInspectWorld, TCommandWorld> effect;

        public Whenever(IWheneverFilter<TInspectWorld, TCommandWorld> filter, IEffect<TInspectWorld, TCommandWorld> effect)
        {
            this.filter = filter;
            this.effect = effect;
        }

        public IEnumerable<InitiatedCommand<TCommandWorld>> GetTriggeredCommands(
            InitiatedCommand<TCommandWorld> command,
            TInspectWorld world)
        {
            if (!filter.TriggersOn(command, world)) return Enumerable.Empty<InitiatedCommand<TCommandWorld>>();

            var nextInitiator = InitiatorFactory.FromEffectOf(command.initiator);
            
            return effect
                .ApplyEffect(command, world)
                .Select(x => new InitiatedCommand<TCommandWorld>(x, nextInitiator));
        }
    }
}