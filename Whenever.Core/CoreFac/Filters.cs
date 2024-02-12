namespace CoreFac
{
    public static class Filters
    {
        
        public static IWheneverFilter<TInspectWorld, TCommandWorld> Compose<TInspectWorld, TCommandWorld>(params IWheneverFilter<TInspectWorld, TCommandWorld>[] filters)
            where TInspectWorld : IInspectWorld
            where TCommandWorld : ICommandWorld
        {
            return new CompositeWheneverFilter<TInspectWorld, TCommandWorld>(filters);
        }
    }
}