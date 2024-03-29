﻿using System.Collections.Generic;

public class StatusEffectCollection<TCommand>
    where TCommand: ICommandWorld
{
    private List<StatusEffect<TCommand>> statusEffects = new();
    
    public int Count => statusEffects.Count;
    
    public void Add(StatusEffect<TCommand> effect)
    {
        statusEffects.Add(effect);
    }

    public IEnumerable<StatusEffect<TCommand>> List()
    {
        return statusEffects;
    }

    public void Clear()
    {
        statusEffects.Clear();
    }
    public void RemoveAllInitiatedBy(CombatantId id)
    {
        statusEffects.RemoveAll(effect => effect.WasInitiatedBy(id));
    }
        
    public IEnumerable<InitiatedCommand<TCommand>> ApplyStatusEffects(CombatantId myId)
    {
        foreach(var statusEffect in statusEffects.ToArray())
        {
            var statusEffectResult = statusEffect.ActivateOn(myId);
            if (statusEffectResult.completion == StatusEffectCompletion.Expired)
            {
                statusEffects.Remove(statusEffect);
            }
            foreach (var command in statusEffectResult.commands)
            {
                yield return new InitiatedCommand<TCommand>(command,  HealthFac.Initiators.From(statusEffect));
            }
        }
    }
}