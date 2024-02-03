namespace WheneverAbstractions._Project.WheneverAbstractions.Commands
{
    public interface ITargetedWorldCommand : IWorldCommand
    {
        public CombatantId Target { get; }
    }
}