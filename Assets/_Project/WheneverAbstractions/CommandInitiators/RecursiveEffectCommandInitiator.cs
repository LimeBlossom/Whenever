namespace WheneverAbstractions._Project.WheneverAbstractions.CommandInitiators
{
    public record RecursiveEffectCommandInitiator: ICommandInitiator
    {
        public int EffectDepth { get; set; }
    }
}