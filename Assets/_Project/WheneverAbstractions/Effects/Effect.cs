using System.Collections.Generic;
using WheneverAbstractions._Project.WheneverAbstractions.Commands;

namespace WheneverAbstractions._Project.WheneverAbstractions.Effects
{
    public interface IEffect
    {
        public IEnumerable<IWorldCommand> ApplyEffect(CombatantId triggerTarget);
    }
}