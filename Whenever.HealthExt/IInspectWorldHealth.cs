﻿using Whenever.Core.WorldInterface;

namespace Whenever.HealthExt
{
    public interface IInspectWorldHealth: IInspectWorld
    {
        public float GetHealth(CombatantId id);
    }
}