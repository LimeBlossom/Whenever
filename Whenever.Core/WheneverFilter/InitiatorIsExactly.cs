using UnityEngine;

public class InitiatorIsExactly<TInspectWorld, TCommandWorld>: IWheneverFilter<TInspectWorld, TCommandWorld>
    where TInspectWorld : IInspectWorld
    where TCommandWorld : ICommandWorld
{
    public readonly CombatantId id;

    public InitiatorIsExactly(CombatantId id)
    {
        this.id = id;
    }
    
    public bool TriggersOn(
        InitiatedCommand<TCommandWorld> initiatedCommand,
        IAliasCombatantIds aliaser,
        TInspectWorld world)
    {
        if (!initiatedCommand.initiator.TryAsOrRecursedFrom<CombatantCommandInitiator>(out var initiator))
        {
            return false;
        }

        if (!world.Contains(initiator.Initiator))
        {
            Debug.LogWarning("Attempted to trigger an effect with a dead initiator, " + initiator.Initiator);
            return false;
        }

        return initiator.Initiator == id;
    }

    public string Describe(IDescriptionContext context)
    {
        return $"{context.InitiatorName} is {context.NameOf(id)}";
    }
}