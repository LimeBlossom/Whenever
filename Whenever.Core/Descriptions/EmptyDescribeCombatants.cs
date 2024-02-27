public class EmptyDescribeCombatants : IDescribeCombatants
{
    public string NameOf(CombatantId id)
    {
        return "Combatant #" + id.ToString();
    }
}