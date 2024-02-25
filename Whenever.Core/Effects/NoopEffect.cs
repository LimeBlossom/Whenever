using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public record NoopEffect<TInspectWorld, TCommandWorld> : IEffect<TInspectWorld, TCommandWorld>
    where TInspectWorld : IInspectWorld
    where TCommandWorld : ICommandWorld
{
    private readonly string description;

    public NoopEffect(string description = "twiddle their thumbs")
    {
        this.description = description;
    }
    
    public IEnumerable<IWorldCommand<TCommandWorld>> ApplyEffect(InitiatedCommand<TCommandWorld> command, TInspectWorld world)
    {
        return Enumerable.Empty<IWorldCommand<TCommandWorld>>();
    }
    public string Describe(IDescriptionContext context)
    {
        return description;
    }
}