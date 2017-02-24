//-----------------------------------------------------------------------
// <copyright file="ClientMessageReceived.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Adapters.Server
{
    using System;
    using MudDesigner.Engine.Game;

    /// <summary>
    /// Arguments provided when the player's connection is closed
    /// </summary>
    public class ConnectionClosedArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionClosedArgs"/> class.
        /// </summary>
        /// <param name="player">The player that was disconnected.</param>
        /// <param name="connection">The connection associated with the player.</param>
        public ConnectionClosedArgs(IPlayer player, IConnection connection)
        {
            this.Player = player;
            this.Connection = connection;
        }

        /// <summary>
        /// Gets the player that was disconnected.
        /// </summary>
        public IPlayer Player { get; }

        /// <summary>
        /// Gets the connection associated with the player.
        /// </summary>
        public IConnection Connection { get; }
    }
}
