/// <summary>
/// An interface used by commands to actually execute changes to the world.
/// </summary>
/// <example>
/// A "damage" command would use a world to call DoDamage(id, damageAmt)
/// </example>
public interface ICommandWorld
{
    void SaySomething(CombatantId id, string message);
}