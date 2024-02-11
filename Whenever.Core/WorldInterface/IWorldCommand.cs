﻿namespace Whenever.Core.WorldInterface
{
    public interface IWorldCommand<in TCommand>
        where TCommand: ICommandWorld
    {
        public void ApplyCommand(TCommand world);
    }
}