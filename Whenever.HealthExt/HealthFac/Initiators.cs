namespace HealthFac
{
    public static class Initiators
    {
        public static ICommandInitiator From<TCommand>(StatusEffect<TCommand> statusEffect)
            where TCommand: ICommandWorld
        {
            return statusEffect.GetInitiator();
        }
    }
}