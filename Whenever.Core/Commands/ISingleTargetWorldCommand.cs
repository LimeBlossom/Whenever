namespace Whenever.Core.Commands
{

    public interface ITargetedWorldCommand : IWorldCommand
    {
        public CombatantId Target { get; }
    }
}