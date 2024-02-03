namespace _Project.WheneverAbstractions
{
    public static class CommandInitiatorFactory
    {
        public static ICommandInitiator From(CombatantId combatantId)
        {
            return new CombatantCommandInitiator()
            {
                Initiator = combatantId,
            };
        }
        
    }
}