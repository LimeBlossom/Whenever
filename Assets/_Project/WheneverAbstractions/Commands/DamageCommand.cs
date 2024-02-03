public record DamageCommand : IWorldCommand
{
    public DamagePackage damagePackage;
    public CombatantId Target { get; set; }
}