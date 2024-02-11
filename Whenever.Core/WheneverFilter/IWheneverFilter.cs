using Whenever.Core.Commands;
using Whenever.Core.WorldInterface;

namespace Whenever.Core.WheneverFilter
{
    public interface IWheneverFilter<in TInspectWorld, TCommandWorld>
        where TInspectWorld : IInspectWorld
        where TCommandWorld : ICommandWorld
    {
        public bool TriggersOn(InitiatedCommand<TCommandWorld> initiatedCommand, TInspectWorld world);
    }
}