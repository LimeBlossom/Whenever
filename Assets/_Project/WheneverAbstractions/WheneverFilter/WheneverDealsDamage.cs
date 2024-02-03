using WheneverAbstractions._Project.WheneverAbstractions.Commands;

namespace WheneverAbstractions._Project.WheneverAbstractions.WheneverFilter
{
    public class WheneverDealsDamage : IWheneverFilter
    {
        public DamageType validDamageType;
        public WheneverCombatantTypeFilter wheneverCombatantTypeFilterType;
        public bool TriggersOn(IWorldCommand command, ICommandInitiator initiator, GlobalCombatWorld world)
        {
            throw new System.NotImplementedException();
        }
    }
}