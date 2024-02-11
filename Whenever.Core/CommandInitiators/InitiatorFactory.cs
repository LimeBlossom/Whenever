using Whenever.Core.WorldInterface;

namespace Whenever.Core.CommandInitiators
{
    public static class InitiatorFactory
    {
        public static ICommandInitiator From(CombatantId combatantId)
        {
            return new CombatantCommandInitiator()
            {
                Initiator = combatantId,
            };
        }
        public static ICommandInitiator FromEffectOf(ICommandInitiator previousInitiator)
        {
            if (previousInitiator is RecursiveEffectCommandInitiator recursiveInitiator)
            {
                return new RecursiveEffectCommandInitiator
                {
                    InitialInitiator = recursiveInitiator.InitialInitiator,
                    EffectDepth = recursiveInitiator.EffectDepth + 1
                };
            }

            return new RecursiveEffectCommandInitiator
            {
                InitialInitiator = previousInitiator,
                EffectDepth = 1
            };
        }
        
        public static ICommandInitiator FromNone(string description = "None")
        {
            return new NoneCommandInitiator(description);
        }
        
    }
}