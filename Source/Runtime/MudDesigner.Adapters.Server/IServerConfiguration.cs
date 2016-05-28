//-----------------------------------------------------------------------
// <copyright file="IServerConfiguration.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Adapters.Server
{
    using System;
    using MudDesigner.Engine;

    /// <summary>
    /// Provides members for configuring how a server will run
    /// </summary>
    public interface IServerConfiguration : IConfiguration
    {
        /// <summary>
        /// Gets or sets a callback that will be invoked during the server startup phase.
        /// </summary>
        Action<IServerContext> OnServerStartup { get; set; }

        /// <summary>
        /// Gets or sets a callback that will be invoked when the server starts shutting down.
        /// </summary>
        Action<IServerContext> OnServerShutdown { get; set; }

        /// <summary>
        /// Gets or sets the port that the server should run on.
        /// </summary>
        int Port { get; set; }

        /// <summary>
        /// Gets or sets the maximum queued connections allowed by the server.
        /// </summary>
        int MaxQueuedConnections { get; set; }

        /// <summary>
        /// Gets the minimum size of the password a player must enter when creating a new character.
        /// </summary>
        int MinimumPasswordSize { get; }

        /// <summary>
        /// Gets the maximum size of the password a player can enter when creating a new character.
        /// </summary>
        int MaximumPasswordSize { get; }

        /// <summary>
        /// Gets the buffer size used to buffer network messages before sending them.
        /// </summary>
        int PreferedBufferSize { get; }

        /// <summary>
        /// Gets or sets the message of the day.
        /// </summary>
        string[] MessageOfTheDay { get; set; }

        /// <summary>
        /// Gets or sets the message displayed when a client connects.
        /// </summary>
        string ConnectedMessage { get; set; }
    }
}
