public class CombatantAlias
{
    private readonly string alias;
    private string readableDescription;
    
    public CombatantAlias(string alias, string readableDescription)
    {
        this.alias = alias;
        this.readableDescription = readableDescription;
    }
    
    public string ReadableDescription => readableDescription;
    

    public override bool Equals(object obj)
    {
        return obj is CombatantAlias other && Equals(other);
    }

    public bool Equals(CombatantAlias other)
    {
        return alias == other.alias;
    }

    public override int GetHashCode()
    {
        return (alias != null ? alias.GetHashCode() : 0);
    }
}