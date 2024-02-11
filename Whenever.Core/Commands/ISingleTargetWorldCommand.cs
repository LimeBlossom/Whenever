using Whenever.Core.WorldInterface;

namespace Whenever.Core.Commands
{
    
    public interface IGenericTargetedWorldCommand<in TCommand> : IWorldCommand<TCommand>
        where TCommand: ICommandWorld
    {
        public CombatantId Target { get; }
    }
}