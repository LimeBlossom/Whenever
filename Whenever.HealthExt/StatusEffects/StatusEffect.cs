public abstract record StatusEffect<TCommand>
    where TCommand: ICommandWorld
{
    private int turnsLeft;
    private readonly ICommandInitiator initiator;

    protected StatusEffect(int turnsLeft, ICommandInitiator initiator)
    {
        this.turnsLeft = turnsLeft;
        this.initiator = initiator;
    }
        
    public ICommandInitiator GetInitiator()
    {
        return initiator;
    }
    
    public bool WasInitiatedBy(CombatantId id)
    {
        if(initiator.TryAsOrRecursedFrom<CombatantCommandInitiator>(out var combatantInitiator))
        {
            return combatantInitiator.Initiator == id;
        }

        return false;
    }
    
    public int GetTurnsLeft()
    {
        return turnsLeft;
    }

    /// <summary>
    /// returns true when 
    /// </summary>
    /// <returns></returns>
    public abstract StatusEffectResult<TCommand> ActivateOn(CombatantId target);

    public string Describe(IDescribeCombatants context)
    {
        return $"{DescribePerTurnEffect(context)} per turn for {GetTurnsLeft()} turns";
    }
    
    protected abstract string DescribePerTurnEffect(IDescribeCombatants context);

    protected bool NextTurnIsExpired()
    {
        if (turnsLeft <= 0)
        {
            return true;
        }

        turnsLeft--;
        return false;
    }
}