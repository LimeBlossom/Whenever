using Whenever.Core.WorldInterface;

namespace Whenever.Core.WheneverTestDemo
{
    public interface ICommandableWorldDemo : ICommandWorld
    {
        Combatant GetCombatantRaw(CombatantId combatantId);
    }
}