using System;
using System.Collections.Generic;
using System.Linq;
using WheneverAbstractions._Project.WheneverAbstractions.CommandInitiators;
using WheneverAbstractions._Project.WheneverAbstractions.Commands;
using WheneverAbstractions._Project.WheneverAbstractions.Effects;
using WheneverAbstractions._Project.WheneverAbstractions.WheneverFilter;

namespace WheneverAbstractions._Project.WheneverAbstractions
{
    public class Whenever
    {
        private IWheneverFilter filter;
        private IEffect effect;

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