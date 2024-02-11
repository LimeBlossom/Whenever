using Whenever.Core;
using Whenever.Core.WorldInterface;

namespace Whenever.DmgTypeEtcExt.Experimental.World
{
    public interface ICommandableWorldDemo : ICommandWorld
    {
        Combatant GetCombatantRaw(CombatantId combatantId);
    }
}