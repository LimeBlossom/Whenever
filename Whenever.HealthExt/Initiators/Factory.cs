using Whenever.Core.CommandInitiators;
using Whenever.HealthExt.StatusEffects;

namespace Whenever.HealthExt.Initiators
{
    public class Factory
    {
        

        public static ICommandInitiator From(StatusEffect statusEffect)
        {
            return statusEffect.GetInitiator();
        }

    }
}