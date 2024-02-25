public interface IWorldCommand<in TCommand>
    where TCommand: ICommandWorld
{
    public void ApplyCommand(TCommand world);
    public string Describe(IDescribeCombatants context);
}