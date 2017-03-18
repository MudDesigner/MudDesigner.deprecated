using System.Threading.Tasks;

namespace MudDesigner.Runtime.Game
{
    public interface IGameCommand
    {
        string Name { get; }

        string Command { get; }

        ISecurityPolicy Policy { get; }

        Task Execute(IPlayer target, IMessageBroker broker, params string[] arguments);
    }
}
