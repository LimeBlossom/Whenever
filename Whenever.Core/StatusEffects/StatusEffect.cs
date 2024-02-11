using System.Collections.Generic;
using Whenever.Core.CommandInitiators;
using Whenever.Core.Commands;
using Whenever.Core.WheneverTestDemo;
using Whenever.Core.WorldInterface;

namespace Whenever.Core.StatusEffects
{
    public enum StatusEffectCompletion
    {
        Expired,
        Active
    }

    public struct StatusEffectResult
    {
        public StatusEffectCompletion completion;
        public IEnumerable<IWorldCommand<ICommandableWorldDemo>> commands;
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