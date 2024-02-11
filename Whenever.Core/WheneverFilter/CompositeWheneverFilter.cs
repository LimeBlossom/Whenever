using System.Linq;
using Whenever.Core.WheneverTestDemo;
using Whenever.Core.WorldInterface;

namespace Whenever.Core.WheneverFilter
{
    public record GenericCompositeWheneverFilter<TInspectWorld, TCommandWorld> : IWheneverFilter<TInspectWorld, TCommandWorld>
        where TInspectWorld : IInspectWorld
        where TCommandWorld : ICommandWorld
    {
        public readonly IWheneverFilter<TInspectWorld, TCommandWorld>[] filters;

        public GenericCompositeWheneverFilter(params IWheneverFilter<TInspectWorld, TCommandWorld>[] filters)
        {
            this.filters = filters;
        }

        public bool TriggersOn(InitiatedCommand<TCommandWorld> initiatedCommand, TInspectWorld world)
        {
            return filters.All(filter => filter.TriggersOn(initiatedCommand, world));
        }
    } 
    
    public record CompositeWheneverFilter: GenericCompositeWheneverFilter<IInspectableWorldDemo, ICommandableWorldDemo>
    {
        public CompositeWheneverFilter(params IWheneverFilter<IInspectableWorldDemo, ICommandableWorldDemo>[] filters) : base(filters)
        {
        }
    }
}