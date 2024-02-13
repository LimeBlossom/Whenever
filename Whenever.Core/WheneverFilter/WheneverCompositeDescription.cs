using System;
using System.Collections.Generic;

namespace DefaultNamespace
{
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
            var consumedFilters = new IWheneverFilter<TI, TC>[filterTypes.Length];

            for (int i = 0; i < consumedFilters.Length; i++)
            {
                var targetType = filterTypes[i];
                var filterIndex = filters.FindIndex(f => FilterMatchesType(targetType, f));
                if(filterIndex <= -1) return null;
                consumedFilters[i] = filters[filterIndex];
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
            return type.IsInstanceOfType(filter);
        }

        protected abstract string DescribeMatch(IWheneverFilter<TI, TC>[] consumedFilters);
    }
}