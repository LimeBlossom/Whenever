public record CombatantCommandInitiator: ICommandInitiator
{
    public CombatantId Initiator { get; set; }
    public string Describe(IDescribeCombatants context)
    {
        return context.NameOf(Initiator);
    }
}