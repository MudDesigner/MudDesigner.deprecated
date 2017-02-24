//-----------------------------------------------------------------------
// <copyright file="UserConnection.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Adapters.Server
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;
    using MudDesigner.Engine;
    using MudDesigner.Engine.Game;

    public static class SocketExtensions
    {    
        public static Task<SocketAsyncEventArgs> PerformAsyncOperation(this Socket socket, Action<SocketAsyncEventArgs> operation, byte[] buffer)
        {
            // Configure the async callback event.
            var completionSource = new TaskCompletionSource<SocketAsyncEventArgs>();
            var asyncArgs = new SocketAsyncEventArgs();
            asyncArgs.Completed += (sender, args) => completionSource.SetResult(args);
            asyncArgs.SetBuffer(buffer, 0, buffer.Length);
            // Invoke what ever socket operation is being used with the args.
            operation(asyncArgs);
            
            return completionSource.Task;
        }
    }

    /// <summary>
    /// Represents a connection to the server
    /// </summary>
    internal sealed class UserConnection : IConnection
    {
        /// <summary>
        /// The buffer size
        /// </summary>
        private readonly int bufferSize;

        /// <summary>
        /// The socket buffer
        /// </summary>
        private byte[] buffer;

        /// <summary>
        /// The outbound message subscription
        /// </summary>
        private ISubscription outboundMessage;

        /// <summary>
        /// The current data being processed
        /// </summary>
        private readonly List<string> currentData;

        /// <summary>
        /// The last chunk received from the socket
        /// </summary>
        private string lastChunk;

        /// <summary>
        /// The player that owns the socket on this connection
        /// </summary>
        private IPlayer player;

        /// <summary>
        /// The socket
        /// </summary>
        private Socket socket;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserConnection" /> class.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="currentConnection">The current connection.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        internal UserConnection(IPlayer player, Socket currentConnection, int bufferSize)
        {
            this.bufferSize = bufferSize;
            this.buffer = new byte[this.bufferSize];
            this.currentData = new List<string>();
            this.lastChunk = string.Empty;
            this.socket = currentConnection;
            this.Connection = socket;

            player.Deleting += this.DisconnectPlayer;
        }

        /// <summary>
        /// Occurs when a client is disconnected.
        /// </summary>
        public event EventHandler<ConnectionClosedArgs> Disconnected;

        /// <summary>
        /// Gets the connection socket.
        /// </summary>
        public Socket Connection { get; private set; }

        /// <summary>
        /// Determines whether this connection is still valid.
        /// </summary>
        public bool IsConnectionValid()
        {
            if (this.socket == null || !this.socket.Connected)
            {
                return false;
            }


            bool pollWasSuccessful = this.socket.Poll(1000, SelectMode.SelectRead);
            bool noBytesReceived = this.socket.Available == 0;
            return !(pollWasSuccessful && noBytesReceived);
        }

        /// <summary>
        /// Initializes the component.
        /// </summary>
        /// <returns>
        /// Returns an awaitable Task
        /// </returns>
        public Task Initialize()
        {
            this.buffer = new byte[this.bufferSize];            
            return this.socket.PerformAsyncOperation(arg => this.socket.ReceiveAsync(arg), this.buffer);
        }

        /// <summary>
        /// Lets this instance know that it is about to go out of scope and disposed.
        /// The instance will perform clean-up of its resources in preperation for deletion.
        /// </summary>
        /// <returns>
        /// Returns an awaitable Task
        /// </returns>
        /// <para>
        /// Informs the component that it is no longer needed, allowing it to perform clean up.
        /// Objects registered to one of the two delete events will be notified of the delete request.
        /// </para>
        public Task Delete() => this.player.Delete();

        /// <summary>
        /// Sends a message to the client
        /// </summary>
        /// <param name="message">The message content</param>
        public Task SendMessage(string content)
        {
            if (!this.IsConnectionValid())
            {
                this.player.Delete();
                return Task.FromResult(0);
            }
            else if (content == null)
            {
                return Task.FromResult(0);
            }

            byte[] buffer = Encoding.ASCII.GetBytes(content);
            return this.socket.PerformAsyncOperation(args => this.socket.SendAsync(args), this.buffer);
        }

        /// <summary>
        /// Called when the socket has received data from the client.
        /// </summary>
        /// <param name="result">The result.</param>
        private async Task ReceiveData()
        {
            if (!this.IsConnectionValid())
            {
                await this.player.Delete();
                return;
            }

            var asyncArg = await this.socket.PerformAsyncOperation(arg => this.socket.ReceiveAsync(arg), this.buffer);
            int bytesRead = asyncArg.BytesTransferred;
            if (bytesRead == 0 || this.buffer.Count() == 0)
            {
                return;
            }

            // TODO: Decode the bits into a string for parsing.
            string commandData = Encoding.UTF8.GetString(this.buffer, 0, bytesRead);
            if (commandData != "\n" || commandData != "\r\0" || commandData != "\r\n" || commandData != "\r")
            {
                return;
            }

            //MessageBrokerFactory.Instance.Publish(new CommandRequestedMessage(commandData, this.player, this.commandProcessor));
            await this.ReceiveData();
        }

        /// <summary>
        /// Event handler to disconnect the socket when the palyer is being deleted.
        /// </summary>
        /// <param name="component">The component being deleted.</param>
        /// <returns>Returns an awaitable Task</returns>
        private Task DisconnectPlayer(IGameComponent component)
        {
            if (this.outboundMessage != null)
            {
                this.outboundMessage.Unsubscribe();
            }

            component.Deleting -= this.DisconnectPlayer;

            this.socket.Shutdown(SocketShutdown.Both);
            this.socket = null;

            var handler = this.Disconnected;
            if (handler == null)
            {
                return Task.FromResult(0);
            }

            handler(this, new ConnectionClosedArgs(this.player, this));
            return Task.FromResult(0);
        }
    }
}
