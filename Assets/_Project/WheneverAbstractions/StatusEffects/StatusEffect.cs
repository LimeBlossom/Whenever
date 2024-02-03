using System.Collections.Generic;
using WheneverAbstractions._Project.WheneverAbstractions.Commands;

namespace WheneverAbstractions._Project.WheneverAbstractions.StatusEffects
{
    public enum StatusEffectCompletion
    {
        Expired,
        Active
    }

    public struct StatusEffectResult
    {
        public StatusEffectCompletion completion;
        public IEnumerable<IWorldCommand> commands;
    }

    public abstract record StatusEffect
    {
        private int turnsLeft;
        private readonly ICommandInitiator initiator;

        protected StatusEffect(int turnsLeft, ICommandInitiator initiator)
        {
            this.turnsLeft = turnsLeft;
            this.initiator = initiator;
        }
        
        public ICommandInitiator GetInitiator()
        {
            return initiator;
        }

        /// <summary>
        /// returns true when 
        /// </summary>
        /// <returns></returns>
        public abstract StatusEffectResult ActivateOn(CombatantId target);

        protected bool NextTurnIsExpired()
        {
            if (turnsLeft <= 0)
            {
                return true;
            }

            turnsLeft--;
            return false;
        }
    }
}