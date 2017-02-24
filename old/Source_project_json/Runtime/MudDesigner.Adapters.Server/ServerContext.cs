//-----------------------------------------------------------------------
// <copyright file="ServerContext.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Adapters.Server
{
    using System.Net.Sockets;

    /// <summary>
    /// Exposes members that can be used to perform operations within the context of a running server operation
    /// </summary>
    public class ServerContext : IServerContext
    {
        /// <summary>
        /// The windows server
        /// </summary>
        private StandardServer windowsServer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerContext"/> class.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="serverSocket">The server socket.</param>
        internal ServerContext(StandardServer server, IServerConfiguration configuration, Socket serverSocket)
        {
            this.ListeningSocket = serverSocket;
            this.Server = server;
            this.windowsServer = server;
            this.Configuration = configuration;
        }

        /// <summary>
        /// Gets or sets the listening socket.
        /// </summary>
        /// <value>
        /// The listening socket.
        /// </value>
        public Socket ListeningSocket { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the operation this context is associated with has been handled.
        /// If it has been handled then the server will stop running the operation itself
        /// </summary>
        public bool IsHandled { get; set; }

        /// <summary>
        /// Gets the server for this context.
        /// </summary>
        public IServer Server { get; private set; }

        /// <summary>
        /// Gets the configuration that was used to configure the server.
        /// </summary>
        public IServerConfiguration Configuration { get; private set; }

        /// <summary>
        /// Sets the state of the server.
        /// </summary>
        /// <param name="status">The status that the server needs to be set with.</param>
        public void SetServerState(ServerStatus status)
        {
            this.windowsServer.Status = status;
        }
    }
}
