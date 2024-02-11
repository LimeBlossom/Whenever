namespace WheneverAbstractions._Project.WheneverAbstractions.Commands
{
    public interface IWorldCommand
    {
        public void ApplyCommand(ICommandableWorld world);
    }
}