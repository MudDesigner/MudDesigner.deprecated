using System.Threading.Tasks;
using MudDesigner.MudEngine.Actors;

namespace MudDesigner.MudEngine.Commanding
{
    public interface IActorCommand
    {
        Task<bool> CanProcessCommand(IPlayer source, string command, params string[] arguments);

        Task<CommandResult> ProcessCommand(IPlayer source, string command, params string[] arguments);
    }
}
