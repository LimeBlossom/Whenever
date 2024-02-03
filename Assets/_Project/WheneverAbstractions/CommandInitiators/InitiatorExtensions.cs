namespace WheneverAbstractions._Project.WheneverAbstractions.CommandInitiators
{
    public static class InitiatorExtensions
    {
        public static bool TryAsOrRecursedFrom<T>(this ICommandInitiator initiator, out T res) where T : ICommandInitiator
        {
            switch (initiator)
            {
                case T t:
                    res = t;
                    return true;
                case RecursiveEffectCommandInitiator { InitialInitiator: T rec }:
                    res = rec;
                    return true;
                default:
                    res = default;
                    return false;
            }
        }
        
    }
}