using System.Linq;
using Whenever.Core.WheneverTestDemo;
using Whenever.Core.WorldInterface;

namespace Whenever.Core.WheneverFilter
{
    public record CompositeWheneverFilter: IWheneverFilter<IInspectableWorldDemo, ICommandableWorldDemo>
    {
        public readonly IWheneverFilter<IInspectableWorldDemo, ICommandableWorldDemo>[] filters;

        public CompositeWheneverFilter(params IWheneverFilter<IInspectableWorldDemo, ICommandableWorldDemo>[] filters)
        {
            this.filters = filters;
        }

        public bool TriggersOn(InitiatedCommand<ICommandableWorldDemo> initiatedCommand, IInspectableWorldDemo world)
        {
            return filters.All(filter => filter.TriggersOn(initiatedCommand, world));
        }
    }
}