public record RecursiveEffectCommandInitiator: ICommandInitiator
{
    public ICommandInitiator InitialInitiator { get; set; }
    public int EffectDepth { get; set; }
}