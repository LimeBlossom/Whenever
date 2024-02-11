namespace Whenever.Core.Commands
{
    public interface IWorldCommand
    {
        public void ApplyCommand(ICommandableWorld world);
    }
}