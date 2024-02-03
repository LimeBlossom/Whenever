namespace _Project.WheneverAbstractions
{
    public interface IEffect
    {
        public void ApplyEffect(DamagePackage damagePackage, Combatant triggerTarget);
    }
}