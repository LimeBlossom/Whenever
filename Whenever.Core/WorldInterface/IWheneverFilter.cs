namespace Whenever.Core.WorldInterface
{
    public interface IWheneverFilter<in TInspectWorld, TCommandWorld>
        where TInspectWorld : IInspectWorld
        where TCommandWorld : ICommandWorld
    {
        public bool TriggersOn(InitiatedCommand<TCommandWorld> initiatedCommand, TInspectWorld world);
    }
}