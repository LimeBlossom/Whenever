using System.Collections.Generic;
using System.Linq;

namespace DefaultNamespace
{
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
        /// Wrapup the given filters into sub-filters to satisfy the description consumers. may attempt to optimize for maximal consumption.
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public CompositeWheneverFilter<TI, TC> GenerateCompositeFilter(IEnumerable<IWheneverFilter<TI, TC>> filters)
        {
            var remainingFilters = filters.ToList();
            var compositeFilters = new List<IWheneverFilter<TI, TC>>();
            while(remainingFilters.Count > 0)
            {
                var consumed = false;
                foreach (var consumer in descriptionConsumers)
                {
                    var compositeFilter = consumer.TryConsumeMatch(remainingFilters);
                    if (compositeFilter != null)
                    {
                        compositeFilters.Add(compositeFilter);
                        consumed = true;
                        break;
                    }
                }

                if (!consumed)
                {
                    compositeFilters.Add(remainingFilters[0]);
                    remainingFilters.RemoveAt(0);
                }
            }

            return new CompositeWheneverFilter<TI, TC>(compositeFilters.ToArray());
        }
    }
}