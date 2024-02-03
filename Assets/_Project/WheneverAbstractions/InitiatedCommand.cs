using WheneverAbstractions._Project.WheneverAbstractions.Commands;

namespace WheneverAbstractions._Project.WheneverAbstractions
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