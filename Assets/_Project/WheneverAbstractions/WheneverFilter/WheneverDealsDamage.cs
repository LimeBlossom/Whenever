using WheneverAbstractions._Project.WheneverAbstractions.Commands;

namespace WheneverAbstractions._Project.WheneverAbstractions.WheneverFilter
{
    
    /// <summary>
    /// Only apply when the command initiator is of a specific combatant type, and when dealing a specific type of damage to some other target.
    /// </summary>
    public class WheneverDealsDamage : IWheneverFilter
    {
        public DamageType validDamageType;
        public WheneverCombatantTypeFilter wheneverCombatantTypeFilterType;
        public bool TriggersOn(InitiatedCommand initiatedCommand, IInspectableWorld world)
        {
            throw new System.NotImplementedException();
        }
    }
}