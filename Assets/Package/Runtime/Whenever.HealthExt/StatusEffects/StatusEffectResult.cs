using System.Collections.Generic;

public struct StatusEffectResult<TCommand>
    where TCommand: ICommandWorld
{
    public StatusEffectCompletion completion;
    public IEnumerable<IWorldCommand<TCommand>> commands;
}