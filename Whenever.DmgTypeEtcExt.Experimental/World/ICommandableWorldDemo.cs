﻿namespace Whenever.DmgTypeEtcExt.Experimental.World
{
    public interface ICommandableWorldDemo : ICommandWorld
    {
        Combatant GetCombatantRaw(CombatantId combatantId);
    }
}