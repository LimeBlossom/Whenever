using WheneverAbstractions._Project.WheneverAbstractions.StatusEffects;

namespace WheneverAbstractions._Project.WheneverAbstractions.CommandInitiators
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

        public static ICommandInitiator From(StatusEffect statusEffect)
        {
            return new StatusEffectCommandInitiator();
        }

        public static ICommandInitiator FromEffectOf(ICommandInitiator previousInitiator)
        {
            if (previousInitiator is RecursiveEffectCommandInitiator recursiveInitiator)
            {
                return new RecursiveEffectCommandInitiator
                {
                    EffectDepth = recursiveInitiator.EffectDepth + 1
                };
            }

            return new RecursiveEffectCommandInitiator
            {
                EffectDepth = 1
            };
        }
        
        public static ICommandInitiator FromNone(string description = "None")
        {
            return new NoneCommandInitiator(description);
        }
        
    }
}