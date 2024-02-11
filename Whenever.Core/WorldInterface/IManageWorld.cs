using System.Collections.Generic;

namespace Whenever.Core.WorldInterface
{
    /// <summary>
    /// Used to manage the world from the main game loop. The only way to trigger changes externally.
    /// </summary>
    public interface IManageWorld<TInspect, TCommand>
        where TInspect : IInspectWorld
        where TCommand : ICommandWorld
    {
        /// <summary>
        /// Initiate a command chain. Whenevers will apply only once, generating an entire new batch of commands from the
        /// previous command batch.
        /// </summary>
        /// <param name="initiatedCommand"></param>
        public void InitiateCommandBatch(IEnumerable<InitiatedCommand<TCommand>> initiatedCommand);
        
        public void AddWhenever(Whenever<TInspect, TCommand> whenever);
    }
}