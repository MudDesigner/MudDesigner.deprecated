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
        private Socket serverSocket;
        private IServer server;
        private IServerConfiguration serverConfig;
        private IPEndPoint readEndPoint;

        private ObjectPool<SocketAsyncEventArgs> socketArgsPool;
        private ArrayPool<byte> bufferPool;
        private const int _bufferPoolBucketSize = 10;

        internal ServerSocketContext(IServer server)
        {
            this.server = server;
            this.serverConfig = server.Configuration;
            this.socketArgsPool = new ObjectPool<SocketAsyncEventArgs>(100, this.CreateSocketArgs, this.ResetSocketArgs);
            this.bufferPool = ArrayPool<byte>.Create(this.server.Configuration.PreferredBufferSize, _bufferPoolBucketSize);
        }

        public event Action<string> MessageReceived;

        public IMessageBroker MessageBroker => this.server.MessageBroker;

        public Task Delete()
        {
            this.serverSocket.Dispose();
            return Task.CompletedTask;
        }

        public Task Initialize()
        {
            this.readEndPoint = new IPEndPoint(IPAddress.Any, this.serverConfig.Port);
            var serverEndPoint = new IPEndPoint(this.serverConfig.HostAddress, this.serverConfig.Port);

            this.serverSocket = new Socket(serverEndPoint.AddressFamily, SocketType.Raw, ProtocolType.Tcp);
            this.serverSocket.Bind(serverEndPoint);

            SocketAsyncEventArgs asyncArgs = this.socketArgsPool.Rent();
            this.serverSocket.ReceiveAsync(asyncArgs);

            return Task.CompletedTask;
        }

        public Task SendMessage(byte[] message)
        {
            this.serverSocket.AcceptAsync(new SocketAsyncEventArgs());
            return Task.CompletedTask;
        }

        private void ReceivedSocketEvent(object sender, SocketAsyncEventArgs e)
        {
            // Rent a new socket arg and start listening for more packets.
            SocketAsyncEventArgs asyncArgs = this.socketArgsPool.Rent();
            this.serverSocket.ReceiveAsync(asyncArgs);

            // Get the data we want from the event args and send it back to the pool for recycling.
            byte[] buffer = e.Buffer;
            int bufferSize = e.BytesTransferred;
            object client = e.UserToken;

            if (bufferSize == 0)
            {
                return;
            }

            // parse the contents of the buffer;
            string contents = Encoding.UTF8.GetString(buffer, 0, bufferSize);
            this.socketArgsPool.Return(e);

            this.MessageReceived?.Invoke(contents);
        }

        private SocketAsyncEventArgs CreateSocketArgs()
        {
            byte[] buffer = this.bufferPool.Rent(this.serverConfig.PreferredBufferSize);
            var asyncArg = new SocketAsyncEventArgs { UserToken = this };
            asyncArg.Completed += new EventHandler<SocketAsyncEventArgs>(this.ReceivedSocketEvent);
            asyncArg.SetBuffer(buffer, 0, this.serverConfig.PreferredBufferSize);
            asyncArg.RemoteEndPoint = this.readEndPoint;

            return asyncArg;
        }

        private void ResetSocketArgs(SocketAsyncEventArgs asyncArgs)
        {
        }
    }
}
