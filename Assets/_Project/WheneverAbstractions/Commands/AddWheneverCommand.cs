public record AddWheneverCommand : IWorldCommand
{
    public Whenever whenever;
    public CombatantId Target { get; set; }
    
    public AddWheneverCommand(CombatantId target, Whenever whenever)
    {
        Target = target;
        this.whenever = whenever;
    }
}