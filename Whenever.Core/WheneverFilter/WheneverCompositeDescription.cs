using System;
using System.Collections.Generic;
using System.Linq;

public abstract class WheneverCompositeDescription<TI, TC>
    where TI : IInspectWorld
    where TC : ICommandWorld
{
    private readonly Type[] filterTypes;
        
    public WheneverCompositeDescription(params System.Type[] filterTypes)
    {
        this.filterTypes = filterTypes;
    }
        
    /// <summary>
    /// Tries to consume some number of filters out of the given list. If it can, then it returns a new composite filter
    /// wrapping them up.
    /// </summary>
    /// <param name="filters"></param>
    /// <returns></returns>
    public CompositeWheneverFilter<TI, TC> TryConsumeMatch(List<IWheneverFilter<TI, TC>> filters)
    {
        var consumedFilterIndexes = new List<int>();

        for (int i = 0; i < filterTypes.Length; i++)
        {
            var targetType = filterTypes[i];

            var validIndexes = filters
                .Select((x, idx) => (x, idx)) // don't consume the same index more than once
                .Where(x => FilterMatchesType(targetType, x.x) && !consumedFilterIndexes.Contains(x.idx))
                .Select(x => (int?) x.idx)
                .ToList();
            
            var filterIndex = validIndexes
                .FirstOrDefault();
            if (filterIndex == null) return null;
            consumedFilterIndexes.Add(filterIndex.Value);
        }
        
        // successfully found a match for all filters
        var consumedFilters = new IWheneverFilter<TI, TC>[filterTypes.Length];
        
        for (int i = 0; i < consumedFilterIndexes.Count; i++)
        {
            consumedFilters[i] = filters[consumedFilterIndexes[i]];
        }
        
        foreach (var consumedFilter in consumedFilters)
        {
            filters.Remove(consumedFilter);
        }
            
        var description = DescribeMatch(consumedFilters);
        return new CompositeWheneverFilter<TI, TC>(description, consumedFilters);
    }

    private bool FilterMatchesType(System.Type type, IWheneverFilter<TI, TC> filter)
    {
        var success = type.IsInstanceOfType(filter);
        return success;
    }

    protected abstract string DescribeMatch(IWheneverFilter<TI, TC>[] consumedFilters);
}