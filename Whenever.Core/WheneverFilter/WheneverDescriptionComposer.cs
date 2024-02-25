using System.Collections.Generic;
using System.Linq;

public class WheneverDescriptionComposer<TI, TC>
    where TI : IInspectWorld
    where TC : ICommandWorld
{
    private readonly WheneverCompositeDescription<TI, TC>[] descriptionConsumers;

    public WheneverDescriptionComposer(params WheneverCompositeDescription<TI, TC>[] descriptionConsumers)
    {
        this.descriptionConsumers = descriptionConsumers;
    }

    /// <summary>
    /// unwrap any composite filters and rewrap them with the description consumers
    /// </summary>
    /// <param name="filters"></param>
    /// <returns></returns>
    public CompositeWheneverFilter<TI, TC> ForceRegenerateComposites(params IWheneverFilter<TI, TC>[] filters)
    {
        var remainingComposites = new Stack<CompositeWheneverFilter<TI, TC>>();
        var results = new List<IWheneverFilter<TI, TC>>();
        foreach (var filter in filters)
        {
            if (filter is CompositeWheneverFilter<TI, TC> composite)
            {
                remainingComposites.Push(composite);
            }
            else
            {
                results.Add(filter);
            }
        }
            
        while (remainingComposites.Count > 0)
        {
            var composite = remainingComposites.Pop();
            foreach (var filter in composite.filters)
            {
                if (filter is CompositeWheneverFilter<TI, TC> subComposite)
                {
                    remainingComposites.Push(subComposite);
                }
                else
                {
                    results.Add(filter);
                }
            }
        }
            
            
        return GenerateCompositeFilter(results.AsEnumerable());
    }
 
    public CompositeWheneverFilter<TI, TC> GenerateCompositeFilter(params IWheneverFilter<TI, TC>[] filters)
    {
        return GenerateCompositeFilter(filters.AsEnumerable());
    }
    /// <summary>
    /// Wrapup the given filters into sub-filters to satisfy the description consumers. may attempt to optimize for maximal consumption.
    /// </summary>
    /// <param name="filters"></param>
    /// <returns></returns>
    public CompositeWheneverFilter<TI, TC> GenerateCompositeFilter(IEnumerable<IWheneverFilter<TI, TC>> filters)
    {
        var remainingFilters = filters.ToList();
        var compositeFilters = new List<IWheneverFilter<TI, TC>>();
        foreach (var consumer in descriptionConsumers)
        {
            while(true)
            {
                var compositeFilter = consumer.TryConsumeMatch(remainingFilters);
                if(compositeFilter == null) break;
                    
                compositeFilters.Add(compositeFilter);
            }
            if(remainingFilters.Count <= 0) break;
        }

        return new CompositeWheneverFilter<TI, TC>(compositeFilters.Concat(remainingFilters).ToArray());
    }
}