public record CombatantsAreSame<TInspectWorld, TCommandWorld>: IWheneverFilter<TInspectWorld, TCommandWorld>
    where TInspectWorld : IInspectWorld
    where TCommandWorld : ICommandWorld
{
    private readonly CombatantAlias variableAlias;
    private readonly CombatantAlias expectedAlias;

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
        var expectedId = aliaser.GetIdForAlias(expectedAlias);
        return variableId == expectedId;
    }
        
    public string Describe(IDescriptionContext context)
    {
        return $"{context.NameOf(variableAlias)} is {context.NameOf(expectedAlias)}";
    }
}