namespace HealthExtInternal
{
    internal record Damage : IGenericTargetedWorldCommand<ICommandWorldHealth>
    {
        public float damage;
        public CombatantId Target { get; }
        
        public Damage(CombatantId target, float damage)
        {
            this.damage = damage;
            Target = target;
        }
        
        public void ApplyCommand(ICommandWorldHealth world)
        {
            world.DoDamage(Target, damage);
        }

        public string Describe()
        {
            return $"deal {damage} damage to {Target}";
        }
    }
}