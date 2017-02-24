using MudDesigner.Runtime.Networking;
using System;

namespace MudDesigner.Runtime.Adapter.Telnet
{
    public static class IGameServerExtensions
    {
        public static IServer AddTelnetServer(this IGame game, Action<IServerConfiguration> configuration = null)
        {
            var serverConfig = new ServerConfiguration();

            configuration?.Invoke(serverConfig);
            var server = new TelnetServer(game, serverConfig);

            game.UseAdapter(server);

            return server;
        }
    }
}
