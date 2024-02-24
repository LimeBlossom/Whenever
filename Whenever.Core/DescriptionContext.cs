﻿using System.Collections.Generic;

public interface IDescribeCombatants
{
    public string NameOf(CombatantId id);
}

public interface IDescriptionContext : IDescribeCombatants
{
    public string InitiatorName { get; }
    public string TargetName { get; }
}

public static class DescriptionContextExtensions
{
    public static string ToTargetAsDirectSubject(this IDescriptionContext context)
    {
        var targetName = context.TargetName;
        if (string.IsNullOrWhiteSpace(targetName))
        {
            return "";
        }
        else
        {
            return " to " + targetName;
        }
    }
}

public class SimpleDescriptionContext : IDescriptionContext
{
    public SimpleDescriptionContext(Dictionary<CombatantId, string> name) : this("the initiator", "the target", name)
    {
    }
    public SimpleDescriptionContext(): this("the initiator", "the target")
    {
    }
    
    public SimpleDescriptionContext(string initiatorName, string targetName)
    {
        this.InitiatorName = initiatorName;
        this.TargetName = targetName;
    }

    public SimpleDescriptionContext(string initiatorName, string targetName, Dictionary<CombatantId, string> names)
    {
        this.InitiatorName = initiatorName;
        this.TargetName = targetName;
        this.names = names ?? new();
    }
    
    private Dictionary<CombatantId, string> names = new();
    public string InitiatorName { get; set; }
    public string TargetName { get; set;  }
    public string NameOf(CombatantId id)
    {
        if(names.TryGetValue(id, out var name))
        {
            return name;
        }
        return "Combatant #" + id.ToString();
    }
}