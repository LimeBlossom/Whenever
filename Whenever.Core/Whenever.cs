using System.Collections.Generic;
using System.Linq;
using CoreFac;

public record Whenever<TInspectWorld, TCommandWorld>
    where TInspectWorld : IInspectWorld
    where TCommandWorld : ICommandWorld
{
    public IWheneverFilter<TInspectWorld, TCommandWorld> filter;
    public IEffect<TInspectWorld, TCommandWorld> effect;

    public Whenever(IWheneverFilter<TInspectWorld, TCommandWorld> filter, IEffect<TInspectWorld, TCommandWorld> effect)
    {
        this.filter = filter;
        this.effect = effect;
    }

    public IEnumerable<InitiatedCommand<TCommandWorld>> GetTriggeredCommands(
        InitiatedCommand<TCommandWorld> command,
        TInspectWorld world)
    {
        if (!filter.TriggersOn(command, world)) return Enumerable.Empty<InitiatedCommand<TCommandWorld>>();

        var nextInitiator = Initiators.FromEffectOf(command.initiator);
            
        return effect
            .ApplyEffect(command, world)
            .Select(x => new InitiatedCommand<TCommandWorld>(x, nextInitiator));
    }
        
    public string Describe()
    {
        return $"whenever {filter.Describe()}; {effect.Describe()}";
    }
}