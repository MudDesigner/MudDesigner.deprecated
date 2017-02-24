//-----------------------------------------------------------------------
// <copyright file="ConnectionFactory.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Adapters.Server
{
    using System;
    using System.Net.Sockets;
    using MudDesigner.Engine.Game;

    /// <summary>
    /// Provides a method for creating a connection to the server
    /// </summary>
    /// <typeparam name="TServer">The type of the server that the connection can communicate with.</typeparam>
    public class ConnectionFactory : IConnectionFactory<StandardServer>
    {
        /// <summary>
        /// Creates the client connection.
        /// </summary>
        /// <param name="player">The player to associate the connection to.</param>
        /// <param name="server">The server that the connection communicates twith.</param>
        /// <returns>Returns an instance of IConnection</returns>
        public IConnection CreateConnection(IPlayer player, StandardServer server)
        {
            Socket playerConnection = server.GetSocketForPlayer(player);
            return new UserConnection(player, playerConnection, server.Configuration.PreferedBufferSize);
        }
    }
}
