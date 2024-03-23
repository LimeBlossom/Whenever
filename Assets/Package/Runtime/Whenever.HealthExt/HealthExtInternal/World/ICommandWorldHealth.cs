namespace HealthExtInternal
{
    internal interface ICommandWorldHealth : ICommandWorld, ICommandWorldStatusEffects<ICommandWorldHealth>
    {
        public void DoDamage(CombatantId id, float health);
    }
}