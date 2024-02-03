namespace WheneverAbstractions._Project.WheneverAbstractions
{
    public record CombatantId
    {
    
        private readonly int id;
    
        public static readonly CombatantId DEFAULT = new CombatantId(0);
    
        public static CombatantId Next(CombatantId id)
        {
            return new CombatantId(id.id + 1);
        }
    
    
        private CombatantId(int id)
        {
            this.id = id;
        }
        
        public override string ToString()
        {
            return id.ToString();
        }
    }
}