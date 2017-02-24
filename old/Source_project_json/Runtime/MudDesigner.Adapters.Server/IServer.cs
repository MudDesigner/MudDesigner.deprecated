//-----------------------------------------------------------------------
// <copyright file="IServer.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using MudDesigner.Engine;
using MudDesigner.Engine.Game;

namespace MudDesigner.Adapters.Server
{
    /// <summary>
    /// Provides members that can be used to interact with a server
    /// </summary>
    public interface IServer : IAdapter, IConfigurable<IServerConfiguration>
    {
        /// <summary>
        /// Gets or sets the owner of the server.
        /// </summary>
        string Owner { get; set; }

        /// <summary>
        /// Gets the port that the server is running on.
        /// </summary>
        int RunningPort { get; }

        /// <summary>
        /// Gets the status of the server.
        /// </summary>
        ServerStatus Status { get; }

        IConnection[] GetConnections();

        IConnection GetConnectionForPlayer(IPlayer player);
    }
}