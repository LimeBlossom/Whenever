using System.Collections.Generic;

public interface IEffect<in TInspectWorld, TCommandWorld> : IDescribableWithContext
    where TInspectWorld : IInspectWorld
    where TCommandWorld : ICommandWorld
{
    public IEnumerable<IWorldCommand<TCommandWorld>> ApplyEffect(
        InitiatedCommand<TCommandWorld> command,
        TInspectWorld world);
}