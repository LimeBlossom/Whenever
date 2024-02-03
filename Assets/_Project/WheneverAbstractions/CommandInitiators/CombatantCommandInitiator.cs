namespace WheneverAbstractions._Project.WheneverAbstractions.CommandInitiators
{
    public record CombatantCommandInitiator: ICommandInitiator
    {
        public CombatantId Initiator { get; set; }
    }
}