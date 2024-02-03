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

    public abstract class StatusEffect
    {
        private int turnsLeft;

        protected StatusEffect(int turnsLeft)
        {
            this.turnsLeft = turnsLeft;
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