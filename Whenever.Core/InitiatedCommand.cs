using Whenever.Core.CommandInitiators;
using Whenever.Core.Commands;

namespace Whenever.Core
{
    public record InitiatedCommand
    {
        public IWorldCommand command;
        public ICommandInitiator initiator;

        public InitiatedCommand(IWorldCommand command, ICommandInitiator initiator)
        {
            this.command = command;
            this.initiator = initiator;
        }
        
        public override string ToString()
        {
            return $"[{initiator}] -> [{command}]";
        }
    }
}