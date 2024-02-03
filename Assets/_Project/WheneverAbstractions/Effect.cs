using System.Collections.Generic;

namespace _Project.WheneverAbstractions
{
    public interface IEffect
    {
        public IEnumerable<IWorldCommand> ApplyEffect(CombatantId triggerTarget);
    }
}