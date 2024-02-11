using Whenever.Core.WheneverTestDemo;
using Whenever.Core.WorldInterface;

namespace Whenever.Core.Commands
{
    
    public interface IGenericTargetedWorldCommand<in TCommand> : IWorldCommand<TCommand>
        where TCommand: ICommandWorld
    {
        public CombatantId Target { get; }
    }

    public interface ITargetedWorldCommand : IGenericTargetedWorldCommand<ICommandableWorldDemo>
    {
    }
}