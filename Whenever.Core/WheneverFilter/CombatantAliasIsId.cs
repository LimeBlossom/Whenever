public record CombatantAliasIsId<TInspectWorld, TCommandWorld>: IWheneverFilter<TInspectWorld, TCommandWorld>
    where TInspectWorld : IInspectWorld
    where TCommandWorld : ICommandWorld
{
    private readonly CombatantAlias variableAlias;
    private readonly CombatantId expectedId;

    public CombatantAlias VariableAlias => variableAlias;
    public CombatantId ExpectedId => expectedId;
    
    public CombatantAliasIsId(CombatantAlias variableAlias, CombatantId expectedId)
    {
        this.variableAlias = variableAlias;
        this.expectedId = expectedId;
    }

    public bool TriggersOn(
        InitiatedCommand<TCommandWorld> initiatedCommand,
        IAliasCombatantIds aliaser,
        TInspectWorld world)
    {
        var variableId = aliaser.GetIdForAlias(variableAlias);
        return variableId == expectedId;
    }
        
    public string Describe(IDescriptionContext context)
    {
        return $"{context.NameOf(variableAlias)} is {context.NameOf(expectedId)}";
    }
}