namespace Whenever.Core.WheneverFilter
{
    public interface IWheneverFilter
    {
        public bool TriggersOn(InitiatedCommand initiatedCommand, IInspectableWorld world);
    }
}