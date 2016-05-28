using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MudDesigner.Engine.Game;

namespace MudDesigner.Adapters.Server
{
    // internal class ServerCommandProcessor : ICommandProcessedEventFactory
    // { 
    //     internal ServerCommandProcessor(IServer server)
    //     {
    //         if (server == null)
    //         {
    //             throw new ArgumentNullException(nameof(server), $"The {typeof(ServerCommandProcessor).Name} requires a reference to a {typeof(IServer).Name} instance.");
    //         }

    //         this.Server = server;
    //     }

    //     internal IServer Server { get; }

    //     public void ProcessCommandForActor(CommandResult state, IPlayer player)
    //     {
    //         IConnection connection = this.Server.GetConnectionForPlayer(player);
    //         connection.SendMessage(state.PlayerMessage);
    //     }
    // }
}
