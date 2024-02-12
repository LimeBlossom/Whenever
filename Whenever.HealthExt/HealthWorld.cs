using System.Collections.Generic;
using JetBrains.Annotations;

public class HealthWorld : IInspectWorldHealth, ICommandWorldHealth
{
    protected Dictionary<CombatantId, HealthCombatant> allCombatants;

    public HealthWorld(List<HealthCombatant> allCombatants)
    {
        this.allCombatants = new();
        var id = CombatantId.DEFAULT;
        foreach (var combatant in allCombatants)
        {
            id = CombatantId.Next(id);
            combatant.id = id;
            this.allCombatants[id] = combatant;
        }
    }

    public float GetHealth(CombatantId id)
    {
        return InspectCombatant(id).health;
    }

    public void DoDamage(CombatantId id, float health)
    {
        var combatant = InspectCombatant(id);
        combatant.health -= health;
    }

    public void AddStatusEffect(CombatantId id, StatusEffect<ICommandWorldHealth> effect)
    {
        var combatant = InspectCombatant(id);
        combatant.statusEffects.Add(effect);
    }
        
    public IEnumerable<CombatantId> AllIds()
    {
        return allCombatants.Keys;
    }

    public HealthCombatant InspectCombatant(CombatantId combatantId)
    {
        return allCombatants[combatantId];
    }

    public List<InitiatedCommand<ICommandWorldHealth>> ApplyAllStatusEffects()
    {
        var resultantCommands = new List<InitiatedCommand<ICommandWorldHealth>>();
        foreach (var combatant in allCombatants)
        {
            resultantCommands.AddRange(combatant.Value.statusEffects.ApplyStatusEffects(combatant.Key));
        }

        return resultantCommands;
    }
        
    public void SaySomething(CombatantId id, string message)
    {
        throw new System.NotImplementedException();
    }
}

public class HealthCombatant
{
    public float health;
    public StatusEffectCollection<ICommandWorldHealth> statusEffects = new();
    [CanBeNull] public CombatantId id; 
        
    public HealthCombatant(float health)
    {
        this.health = health;
    }
}