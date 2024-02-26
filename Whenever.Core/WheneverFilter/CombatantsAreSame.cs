using UnityEngine;

public record CombatantsAreSame<TInspectWorld, TCommandWorld>: IWheneverFilter<TInspectWorld, TCommandWorld>
    where TInspectWorld : IInspectWorld
    where TCommandWorld : ICommandWorld
{
    /// <summary>
    /// The alias which is expected to change more often, leaving the <see cref="expectedAlias"/> constant through time
    /// </summary>
    /// <remarks>
    /// For example, this may be <see cref="StandardAliases.Target"/> during an operation which decides which target to point the card at.
    /// </remarks>
    /// <remarks>
    /// This is primarily relevant only for descriptive text, functionally these variables are symmetric.
    /// </remarks>
    public readonly CombatantAlias variableAlias;
    /// <summary>
    /// The alias which is expected to remain constant through time, leaving the <see cref="variableAlias"/> to change
    /// </summary>
    /// <remarks>
    /// This is primarily relevant only for descriptive text, functionally these variables are symmetric.
    /// </remarks>
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