namespace WheneverAbstractions._Project.WheneverAbstractions.CommandInitiators
{
    public record NoneCommandInitiator(string descriptiveName): ICommandInitiator
    {
        public string descriptiveName { get; } = descriptiveName;
    }
}