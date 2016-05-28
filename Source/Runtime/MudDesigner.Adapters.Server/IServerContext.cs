//-----------------------------------------------------------------------
// <copyright file="IServerContext.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Adapters.Server
{
    /// <summary>
    /// Exposes members that can be used to perform operations within the context of a running server operation
    /// </summary>
    public interface IServerContext
    {
        /// <summary>
        /// Gets or sets a value indicating whether the operation this context is associated with has been handled.
        /// If it has been handled then the server will stop running the operation itself
        /// </summary>
        bool IsHandled { get; set; }

        /// <summary>
        /// Gets the server for this context.
        /// </summary>
        IServer Server { get; }

        /// <summary>
        /// Gets the configuration that was used to configure the server.
        /// </summary>
        IServerConfiguration Configuration { get; }

        /// <summary>
        /// Sets the state of the server.
        /// </summary>
        /// <param name="status">The status that the server needs to be set with.</param>
        void SetServerState(ServerStatus status);
    }
}
