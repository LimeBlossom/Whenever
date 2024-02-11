namespace WheneverAbstractions._Project.WheneverAbstractions.WheneverFilter
{
    public interface IWheneverFilter
    {
        public bool TriggersOn(InitiatedCommand initiatedCommand, IInspectableWorld world);
    }
}