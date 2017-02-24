//-----------------------------------------------------------------------
// <copyright file="ServerConfiguration.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Adapters.Server
{
    using System;
    using System.Collections.Generic;
    using MudDesigner.Engine;

    /// <summary>
    /// Provides members for configuring how a server will run
    /// </summary>
    public class ServerConfiguration : IServerConfiguration
    {
        /// <summary>
        /// The adapters registered with this server.
        /// </summary>
        private List<IAdapter> adapters;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerConfiguration"/> class.
        /// The server is registered with a minimum password size of 6 characters, maximum password size of 16 characters and a default socket buffer size of 8192b
        /// </summary>
        public ServerConfiguration() : this(16, 6, 256)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerConfiguration"/> class.
        /// </summary>
        /// <param name="maxPasswordSize">Maximum size of the password.</param>
        /// <param name="minPasswordSize">Minimum size of the password.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        public ServerConfiguration(int maxPasswordSize, int minPasswordSize, int bufferSize)
        {
            this.adapters = new List<IAdapter>();

            this.MaximumPasswordSize = maxPasswordSize;
            this.MinimumPasswordSize = minPasswordSize;
            this.PreferedBufferSize = bufferSize;
        }

        /// <summary>
        /// Gets or sets a callback that will be invoked during the server startup phase.
        /// </summary>
        public Action<IServerContext> OnServerStartup { get; set; }

        /// <summary>
        /// Gets or sets a callback that will be invoked when the server starts shutting down.
        /// </summary>
        public Action<IServerContext> OnServerShutdown { get; set; }

        /// <summary>
        /// Gets or sets the message displayed when a client connects.
        /// </summary>
        public string ConnectedMessage { get; set; }

        /// <summary>
        /// Gets the maximum size of the password a player can enter when creating a new character.
        /// </summary>
        public int MaximumPasswordSize { get; private set; }

        /// <summary>
        /// Gets or sets the maximum queued connections allowed by the server.
        /// </summary>
        public int MaxQueuedConnections { get; set; }

        /// <summary>
        /// Gets or sets the message of the day.
        /// </summary>
        public string[] MessageOfTheDay { get; set; }

        /// <summary>
        /// Gets the minimum size of the password a player must enter when creating a new character.
        /// </summary>
        public int MinimumPasswordSize { get; private set; }

        /// <summary>
        /// Gets or sets the port that the server should run on.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets the buffer size used to buffer network messages before sending them.
        /// </summary>
        public int PreferedBufferSize { get; private set; }

        /// <summary>
        /// Gets the game adapter components that have been registered.
        /// </summary>
        /// <returns>Returns an array of adapter components</returns>
        public IAdapter[] GetAdapters()
        {
            return this.adapters.ToArray();
        }

        /// <summary>
        /// Tells the game configuration that specific adapter components must be used by the game.
        /// </summary>
        /// <param name="adapters">The adapters.</param>
        public void UseAdapters(IEnumerable<IAdapter> adapters)
        {
            foreach(IAdapter adapter in adapters)
            {
                this.UseAdapter(adapter);
            }
        }

        /// <summary>
        /// Tells the game configuration that a specific adapter component must be used by the game.
        /// A new instance of TConfigComponent will be created when the game starts.
        /// </summary>
        /// <typeparam name="TAdapter">The type of the adapter component to use.</typeparam>
        public void UseAdapter<TAdapter>() where TAdapter : class, IAdapter, new()
        {
            this.adapters.Add(new TAdapter());
        }

        /// <summary>
        /// Tells the game configuration that a specific adapter component must be used by the game.
        /// </summary>
        /// <typeparam name="TAdapter">The type of the adapter component.</typeparam>
        /// <param name="component">The component instance you want to use.</param>
        public void UseAdapter<TAdapter>(TAdapter component) where TAdapter : class, IAdapter
        {
            this.adapters.Add(component);
        }
    }
}
