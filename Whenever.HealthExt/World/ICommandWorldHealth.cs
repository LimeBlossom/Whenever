public interface ICommandWorldHealth : ICommandWorld
{
    public void DoDamage(CombatantId id, float health);
    public void AddStatusEffect(CombatantId id, StatusEffect<ICommandWorldHealth> effect);
}