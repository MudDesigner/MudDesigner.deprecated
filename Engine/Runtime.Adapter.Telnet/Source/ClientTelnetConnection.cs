using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using MudDesigner.Runtime.Game;
using MudDesigner.Runtime.Networking;

namespace MudDesigner.Runtime.Adapter.Telnet
{
    public class ClientTelnetConnection : IConnection
    {
        private IServerContext serverContext;
        private Socket clientSocket;

        public ClientTelnetConnection(IServerContext serverContext, SocketAsyncEventArgs connectedEventArgs)
        {
            this.clientSocket = connectedEventArgs.AcceptSocket;
            this.serverContext = serverContext;
        }

        public event Action<ConnectionClosedArgs> Disconnected;

        public IMessageBroker MessageBroker => this.serverContext.MessageBroker;

        public Task Delete()
        {
            throw new NotImplementedException();
        }

        public Task Disconnect()
        {
            throw new NotImplementedException();
        }

        public Task Initialize()
        {
            throw new NotImplementedException();
        }

        public bool IsConnectionValid()
        {
            throw new NotImplementedException();
        }

        public Task SendMessage(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            Socket socket = this.clientSocket;
            var args = new SocketAsyncEventArgs();

            args.SetBuffer(buffer, 0, buffer.Length);
            args.RemoteEndPoint = socket.RemoteEndPoint;

            socket.SendAsync(args);
            return Task.CompletedTask;
        }
    }
}
