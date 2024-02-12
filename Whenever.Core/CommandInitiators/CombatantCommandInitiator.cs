public record CombatantCommandInitiator: ICommandInitiator
{
    public CombatantId Initiator { get; set; }
}