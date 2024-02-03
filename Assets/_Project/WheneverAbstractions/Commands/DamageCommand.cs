namespace WheneverAbstractions._Project.WheneverAbstractions.Commands
{
    public record DamageCommand : ITargetedWorldCommand
    {
        public DamagePackage damagePackage;
        public CombatantId Target { get; set; }
        public void ApplyCommand(ICommandableWorld world)
        {
            var target = world.GetCombatantRaw(Target);
            var withResistance = target.damageable.ApplyResistances(damagePackage);
            target.health.Reduce(withResistance.damageAmount);
        }
    }
}