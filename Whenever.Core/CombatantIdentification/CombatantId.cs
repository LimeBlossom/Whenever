public record CombatantId
{
    private readonly int id;
    // allows usage of the id field to store hashes safely, alongside the default sequencing constructor
    private readonly string hashedName;
    
    public static readonly CombatantId DEFAULT = new CombatantId(1, null);
    public static readonly CombatantId INVALID = default;
    
    public static CombatantId Next(CombatantId id)
    {
        return new CombatantId(id.id + 1, null);
    }

    public static CombatantId Hashed(string name)
    {
        return new CombatantId(name.GetHashCode(), name);
    }

    private CombatantId(int id, string hashedName)
    {
        this.id = id;
        this.hashedName = hashedName;
    }
    
        
    public override string ToString()
    {
        return hashedName ?? id.ToString();
    }
}