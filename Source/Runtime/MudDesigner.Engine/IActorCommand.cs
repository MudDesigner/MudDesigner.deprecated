using System.Threading.Tasks;
using MudDesigner.Engine.Game;

namespace MudDesigner.Engine
{
    public interface IActorCommand
    {
        /// <summary>
        /// Determines if another IActor can force this command to be run by a different IActor.
        /// </summary>
        bool IsCommandDelegationAllowed { get; }
        
        bool IsCompleted { get; }
        
        /// <summary>
        /// Gets if this command is part of a command chain. 
        /// An example of this is the LoginCommand chaining multiple inputs together.
        // </summary>
        bool IsChainedCommand { get; }
        
        /// <summary>
        /// Gets whether this is a System command or not.
        /// System commands may not be executed by an IActor directly.
        /// They are executed indirectly either by another command or by the system passing user input into the command.
        // </summary>
        bool IsSystemCommand { get; }
        
        IActorCommand GetNextCommand();
        
        IActorCommand GetPreviousCommand();
        
        string[] GetCommandAliases();
        
        Task<bool> CanProcessCommand(ISecurity security, IMob source, string command, params string[] arguments);
        
        Task<bool> CanProcessCommand(ISecurity security, IMob source, params string[] arguments);

        Task<CommandResult> ProcessCommand(IMob source, string command, params string[] arguments);
        
        Task<CommandResult> ProcessCommand(IMob source, params string[] arguments);
    }
}
