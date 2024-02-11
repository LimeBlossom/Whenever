using System.Linq;

namespace WheneverAbstractions._Project.WheneverAbstractions.WheneverFilter
{
    public record CompositeWheneverFilter: IWheneverFilter
    {
        public readonly IWheneverFilter[] filters;

        public CompositeWheneverFilter(params IWheneverFilter[] filters)
        {
            this.filters = filters;
        }

        public bool TriggersOn(InitiatedCommand initiatedCommand, IInspectableWorld world)
        {
            return filters.All(filter => filter.TriggersOn(initiatedCommand, world));
        }
    }
}