public record CombatantId
{
    private readonly int id;
    // allows usage of the id field to store hashes safely, alongside the default sequencing constructor
    private readonly bool isHashed;
    
    public static readonly CombatantId DEFAULT = new CombatantId(1, false);
    public static readonly CombatantId INVALID = default;
    
    public static CombatantId Next(CombatantId id)
    {
        return new CombatantId(id.id + 1, false);
    }

    public static CombatantId Hashed(string name)
    {
        return new CombatantId(name.GetHashCode(), true);
    }

    public CombatantId Next()
    {
        return CombatantId.Next(this);
    }
    
    private CombatantId(int id, bool isHashed)
    {
        this.id = id;
        this.isHashed = isHashed;
    }
    
        
    public override string ToString()
    {
        return id.ToString();
    }
}