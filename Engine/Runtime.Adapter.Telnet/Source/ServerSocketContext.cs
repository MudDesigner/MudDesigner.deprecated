using MudDesigner.Runtime.Networking;
using System;
using System.Buffers;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.Runtime.Adapter.Telnet
{
    internal class ServerSocketContext : IServerContext
    {
        const byte _carriageReturn = 13;
        const byte _newLine = 10;

        // Telnet protocol commands, found under "TELNET COMMAND STRUCTURE" https://tools.ietf.org/html/rfc854
        // TODO: Should this be an enum?
        const byte _subnegotiation = 240;
        const byte _will = 251;
        const byte _wont = 252;
        const byte _do = 253;
        const byte _dont = 254;
        const byte _interpretAsCommand = 255;

        private bool isDisposing;
        private TelnetServer server;
        private IServerConfiguration serverConfig;

        private ObjectPool<SocketAsyncEventArgs> socketArgsPool;
        private ArrayPool<byte> bufferPool;
        private const int _bufferPoolBucketSize = 10;
        private IPEndPoint serverEndPoint;

        internal ServerSocketContext(TelnetServer server)
        {
            this.server = server;
            this.serverConfig = server.Configuration;

            this.socketArgsPool = new ObjectPool<SocketAsyncEventArgs>(100, this.CreateSocketArgs);
            this.bufferPool = ArrayPool<byte>.Create(this.server.Configuration.PreferredBufferSize, _bufferPoolBucketSize);
        }

        public IMessageBroker MessageBroker => this.server.MessageBroker;

        public Socket ServerSocket { get; private set; }

        public Task Delete()
        {
            this.isDisposing = true;
            this.ServerSocket.Dispose();
            return Task.CompletedTask;
        }

        public Task Initialize()
        {
            this.serverEndPoint = new IPEndPoint(IPAddress.Any, this.serverConfig.Port);
            this.ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.ServerSocket.Bind(serverEndPoint);
            this.ServerSocket.Listen(20);

            this.ListenForConnection();
            return Task.CompletedTask;
        }

        public Task SendMessage(byte[] message)
        {
            this.ServerSocket.AcceptAsync(new SocketAsyncEventArgs());
            return Task.CompletedTask;
        }

        private void ListenForConnection()
        {
            if (this.isDisposing)
            {
                return;
            }

            var socketAsyncArgs = this.socketArgsPool.Rent();
            this.ServerSocket.AcceptAsync(socketAsyncArgs);
        }

        private SocketAsyncEventArgs CreateSocketArgs()
        {
            byte[] buffer = this.bufferPool.Rent(this.serverConfig.PreferredBufferSize);
            var asyncArg = new SocketAsyncEventArgs { UserToken = this };
            asyncArg.Completed += new EventHandler<SocketAsyncEventArgs>(this.ReceivedSocketEvent);
            asyncArg.SetBuffer(buffer, 0, this.serverConfig.PreferredBufferSize);
            asyncArg.RemoteEndPoint = this.serverEndPoint;

            return asyncArg;
        }

        private string GetDecodedContent(SocketAsyncEventArgs asyncArgs)
        {
            int lastIndex = 0;
            for (int index = 0; index < asyncArgs.BytesTransferred; index++)
            {
                if (asyncArgs.Buffer[index] == _interpretAsCommand)
                {
                    index += 2;
                    continue;
                }

                lastIndex = index;
            }

            if (lastIndex > asyncArgs.BytesTransferred - 1)
            {
                return null;
            }

            return Encoding.UTF8.GetString(asyncArgs.Buffer, lastIndex, asyncArgs.BytesTransferred - lastIndex);
        }

        private void ReceivedSocketEvent(object sender, SocketAsyncEventArgs e)
        {
            switch(e.LastOperation)
            {
                case SocketAsyncOperation.Accept:
                    this.ClientConnected(e);
                    break;
                case SocketAsyncOperation.Receive:
                    this.ReceiveSocketData(e);
                    break;
                case SocketAsyncOperation.Disconnect:
                    this.DisconnectSocket(e);
                    break;
            }
        }

        private void ClientConnected(SocketAsyncEventArgs e)
        {
            IConnection connection = new ClientTelnetConnection(this, e);
            var connectionMessage = new ClientConnectedMessage(connection);
            this.MessageBroker.Publish(connectionMessage);

            ListenForConnection();
            e.AcceptSocket.ReceiveAsync(e);
        }

        private void ReceiveSocketData(SocketAsyncEventArgs asyncArgs)
        {
            byte[] buffer = asyncArgs.Buffer;
            int bufferSize = asyncArgs.BytesTransferred;
            object client = asyncArgs.UserToken;

            if (bufferSize == 0)
            {
                return;
            }

            if (bufferSize > 2 && buffer[bufferSize-2] != _carriageReturn && buffer[bufferSize-1] != _newLine)
            {
                // If we have not received the entire message, queue for the next receive call.
            }

            // parse the contents of the buffer;
            string contents = Encoding.UTF8.GetString(buffer, 0, bufferSize);
            asyncArgs.AcceptSocket.ReceiveAsync(asyncArgs);

            this.MessageBroker.Publish(new NetworkMessageReceived(contents));
        }

        private void DisconnectSocket(SocketAsyncEventArgs asyncArgs)
        {
            this.socketArgsPool.Return(asyncArgs);
        }
    }
}
