using Whenever.Core.CommandInitiators;
using Whenever.Core.WorldInterface;
using Whenever.HealthExt.StatusEffects;

namespace Whenever.HealthExt.Initiators
{
    public class Factory
    {
        public static ICommandInitiator From<TCommand>(StatusEffect<TCommand> statusEffect)
            where TCommand: ICommandWorld
        {
            return statusEffect.GetInitiator();
        }

    }
}