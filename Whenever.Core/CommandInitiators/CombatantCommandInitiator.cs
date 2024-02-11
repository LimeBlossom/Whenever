using Whenever.Core.WorldInterface;

namespace Whenever.Core.CommandInitiators
{
    public record CombatantCommandInitiator: ICommandInitiator
    {
        public CombatantId Initiator { get; set; }
    }
}