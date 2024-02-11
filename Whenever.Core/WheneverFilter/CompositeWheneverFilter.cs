using System.Linq;
using Whenever.Core.Commands;
using Whenever.Core.WorldInterface;

namespace Whenever.Core.WheneverFilter
{
    public record CompositeWheneverFilter<TInspectWorld, TCommandWorld> : IWheneverFilter<TInspectWorld, TCommandWorld>
        where TInspectWorld : IInspectWorld
        where TCommandWorld : ICommandWorld
    {
        public readonly IWheneverFilter<TInspectWorld, TCommandWorld>[] filters;

        public CompositeWheneverFilter(params IWheneverFilter<TInspectWorld, TCommandWorld>[] filters)
        {
            this.filters = filters;
        }

        public bool TriggersOn(InitiatedCommand<TCommandWorld> initiatedCommand, TInspectWorld world)
        {
            return filters.All(filter => filter.TriggersOn(initiatedCommand, world));
        }
    }
}