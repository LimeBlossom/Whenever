namespace Whenever.Core.CommandInitiators
{
    public record NoneCommandInitiator(string descriptiveName): ICommandInitiator
    {
        public string descriptiveName { get; } = descriptiveName;
    }
}