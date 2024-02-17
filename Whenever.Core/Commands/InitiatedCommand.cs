using System.Runtime.CompilerServices;

public record InitiatedCommand<TCommand> where TCommand: ICommandWorld
{
    public readonly IWorldCommand<TCommand> command;
    public readonly ICommandInitiator initiator;

    public InitiatedCommand(IWorldCommand<TCommand> command, ICommandInitiator initiator)
    {
        this.command = command;
        this.initiator = initiator;
    }
        
    public override string ToString()
    {
        return $"[{initiator}] -> [{command}]";
    }

    public string Describe(IDescribeCombatants context)
    {
        return $"{initiator.Describe(context)} will {command.Describe(context)}";
    }

    public override int GetHashCode()
    {
        // hash code via object reference
        return RuntimeHelpers.GetHashCode(command) ^ RuntimeHelpers.GetHashCode(initiator);
    }
}