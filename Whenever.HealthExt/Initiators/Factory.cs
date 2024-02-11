using Whenever.Core.CommandInitiators;
using Whenever.Core.StatusEffects;
using Whenever.Core.WorldInterface;

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