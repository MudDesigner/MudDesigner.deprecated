using System;
using System.Threading.Tasks;
using MudDesigner.MudEngine.Actors;

namespace MudDesigner.MudEngine.Commanding
{
    [CommandName("South")]
    [CommandName("North")]
    [CommandName("East")]
    [CommandName("West")]
    public class WalkCommand : IActorCommand
    {
        public Task<bool> CanProcessCommand(IPlayer source, string command, params string[] arguments)
        {
            throw new NotImplementedException();
        }
        public Task<CommandResult> ProcessCommand(IPlayer source, string command, params string[] arguments)
        {
            throw new NotImplementedException();
        }
    }
}
