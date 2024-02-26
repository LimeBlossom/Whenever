using UnityEngine;

public record CombatantsAreSame<TInspectWorld, TCommandWorld>: IWheneverFilter<TInspectWorld, TCommandWorld>
    where TInspectWorld : IInspectWorld
    where TCommandWorld : ICommandWorld
{
    public readonly CombatantAlias variableAlias;
    public readonly CombatantAlias expectedAlias;

    public CombatantsAreSame(CombatantAlias variableAlias, CombatantAlias expectedAlias)
    {
        this.variableAlias = variableAlias;
        this.expectedAlias = expectedAlias;
    }

    public bool TriggersOn(
        InitiatedCommand<TCommandWorld> initiatedCommand,
        IAliasCombatantIds aliaser,
        TInspectWorld world)
    {
        var variableId = aliaser.GetIdForAlias(variableAlias);
        if (variableId == null)
        {
            Debug.LogWarning($"Could not find target for alias '{variableAlias}'");
            return false;
        }
        var expectedId = aliaser.GetIdForAlias(expectedAlias);
        if (expectedId == null)
        {
            Debug.LogWarning($"Could not find target for alias '{expectedAlias}'");
            return false;
        }
        return variableId == expectedId;
    }
        
    public string Describe(IDescriptionContext context)
    {
        return $"{context.NameOf(variableAlias)} is {context.NameOf(expectedAlias)}";
    }
}