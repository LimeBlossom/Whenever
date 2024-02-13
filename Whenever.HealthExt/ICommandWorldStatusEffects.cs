public interface ICommandWorldStatusEffects<T>: ICommandWorld
    where T: ICommandWorldStatusEffects<T>
{
    public void AddStatusEffect(CombatantId id, StatusEffect<T> effect);
}