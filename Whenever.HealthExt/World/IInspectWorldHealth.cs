using Whenever.Core;
using Whenever.Core.WorldInterface;

namespace Whenever.HealthExt.World
{
    public interface IInspectWorldHealth: IInspectWorld
    {
        public float GetHealth(CombatantId id);
    }
}