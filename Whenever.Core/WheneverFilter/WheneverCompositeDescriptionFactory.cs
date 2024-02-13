using System;

public static class WheneverCompositeDescriptionFactory<TI, TC>
    where TI : IInspectWorld
    where TC : ICommandWorld
{
    public static WheneverCompositeDescription<TI, TC> Create<T1, T2>(Func<T1, T2, string> describe)
        where T1: IWheneverFilter<TI, TC>
        where T2: IWheneverFilter<TI, TC>
    {
        return new LambdaCompositeDescription<TI, TC>((filters) => describe((T1)filters[0], (T2)filters[1]), typeof(T1), typeof(T2));
    }
        
    public static WheneverCompositeDescription<TI, TC> Create<T1, T2, T3>(Func<T1, T2, T3, string> describe)
        where T1: IWheneverFilter<TI, TC>
        where T2: IWheneverFilter<TI, TC>
        where T3: IWheneverFilter<TI, TC>
    {
        return new LambdaCompositeDescription<TI, TC>((filters) => describe((T1)filters[0], (T2)filters[1], (T3)filters[2]), typeof(T1), typeof(T2), typeof(T3));
    }
        
    public static WheneverCompositeDescription<TI, TC> Create<T1, T2, T3, T4>(Func<T1, T2, T3, T4, string> describe)
        where T1: IWheneverFilter<TI, TC>
        where T2: IWheneverFilter<TI, TC>
        where T3: IWheneverFilter<TI, TC>
        where T4: IWheneverFilter<TI, TC>
    {
        return new LambdaCompositeDescription<TI, TC>((filters) => describe((T1)filters[0], (T2)filters[1], (T3)filters[2], (T4)filters[3]), typeof(T1), typeof(T2), typeof(T3), typeof(T4));
    }
}
    
internal class LambdaCompositeDescription<TI, TC>: WheneverCompositeDescription<TI, TC>
    where TI : IInspectWorld
    where TC : ICommandWorld
{
    private readonly Func<IWheneverFilter<TI, TC>[], string> describe;

    public LambdaCompositeDescription(Func<IWheneverFilter<TI, TC>[], string> describe, params System.Type[] types) : base(types)
    {
        this.describe = describe;
    }
        
    protected override string DescribeMatch(IWheneverFilter<TI, TC>[] consumedFilters)
    {
        return describe(consumedFilters);
    }
}