public record NoneCommandInitiator(string descriptiveName): ICommandInitiator
{
    public string descriptiveName { get; } = descriptiveName;
    public string Describe(IDescribeCombatants context)
    {
        return "the gods above";
    }
}