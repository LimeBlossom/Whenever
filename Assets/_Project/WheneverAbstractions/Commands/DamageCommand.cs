namespace WheneverAbstractions._Project.WheneverAbstractions.Commands
{
    public record DamageCommand : ITargetedWorldCommand
    {
        public DamagePackage damagePackage;
        public CombatantId Target { get; set; }
    }
}