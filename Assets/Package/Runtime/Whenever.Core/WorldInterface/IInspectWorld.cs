﻿using System.Collections.Generic;
using System.Linq;

/// <summary>
/// An interface used by Effects to determine if, or how, to generate a Command.
/// </summary>
/// <example>
/// A Random boulder command would inspect a list of all combatant ids, and generate a "boulder" command for a random combatant.
/// </example>
/// <example>
/// A damage adjacent command would use a world inspected with knowledge about adjacency
/// to query for combatants adjacent to the target combatant, and generate a "damage" command for each adjacent combatant.
/// </example>
public interface IInspectWorld
{
    IEnumerable<CombatantId> AllIds();
}

public static class IInspectWorldExtensions
{
    public static bool Contains(this IInspectWorld world, CombatantId id)
    {
        return world.AllIds().Contains(id);
    }
}