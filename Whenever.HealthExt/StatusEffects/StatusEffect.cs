using Whenever.Core;
using Whenever.Core.CommandInitiators;
using Whenever.Core.WorldInterface;
using Whenever.HealthExt.World;

namespace Whenever.HealthExt.StatusEffects
{
    public abstract record StatusEffect<TCommand>
        where TCommand: ICommandWorld
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
        public abstract StatusEffectResult<TCommand> ActivateOn(CombatantId target);

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