namespace HealthExtInternal
{
    internal interface IInspectWorldHealth: IInspectWorld
    {
        public float GetHealth(CombatantId id);
    }
}