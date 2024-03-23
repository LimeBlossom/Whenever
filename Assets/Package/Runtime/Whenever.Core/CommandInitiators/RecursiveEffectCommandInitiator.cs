public record RecursiveEffectCommandInitiator: ICommandInitiator
{
    public ICommandInitiator InitialInitiator { get; set; }
    public int EffectDepth { get; set; }
    public string Describe(IDescribeCombatants context)
    {
        return InitialInitiator.Describe(context) + $" ({EffectDepth})";
    }
}