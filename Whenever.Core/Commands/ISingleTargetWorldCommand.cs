using Whenever.Core.WheneverTestDemo;
using Whenever.Core.WorldInterface;

namespace Whenever.Core.Commands
{

    public interface ITargetedWorldCommand : IWorldCommand<ICommandableWorldDemo>
    {
        public CombatantId Target { get; }
    }
}