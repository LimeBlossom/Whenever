using Whenever.Core.CommandInitiators;
using Whenever.Core.WorldInterface;

namespace Whenever.Core.Commands
{
    public record InitiatedCommand<TCommand> where TCommand: ICommandWorld
    {
        public IWorldCommand<TCommand> command;
        public ICommandInitiator initiator;

        public InitiatedCommand(IWorldCommand<TCommand> command, ICommandInitiator initiator)
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