//-----------------------------------------------------------------------
// <copyright file="IConnection.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Adapters.Server
{
    using System;
    using System.Threading.Tasks;
    using MudDesigner.Engine;

    /// <summary>
    /// Represents a connection to the server
    /// </summary>
    public interface IConnection : IInitializableComponent
    {
        /// <summary>
        /// Occurs when a client is disconnected.
        /// </summary>
        event EventHandler<ConnectionClosedArgs> Disconnected;

        /// <summary>
        /// Determines whether this connection is still valid.
        /// </summary>
        /// <returns>Returns true if the connection can continue to be used</returns>
        bool IsConnectionValid();

        /// <summary>
        /// Sends a message to the client
        /// </summary>
        /// <param name="message">The message content</param>
        Task SendMessage(string message);
    }
}
