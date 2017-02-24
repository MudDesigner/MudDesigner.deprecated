//-----------------------------------------------------------------------
// <copyright file="ClientMessageReceived.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using MudDesigner.Engine;

namespace MudDesigner.Adapters.Server
{
    /// <summary>
    /// MessageBroker message type for when the server receives data from the client
    /// </summary>
    public class ClientMessageReceived : MessageBase<ClientData>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientMessageReceived"/> class.
        /// </summary>
        /// <param name="data">The data sent by the client.</param>
        public ClientMessageReceived(ClientData data)
        {
            this.Content = data;
        }
    }
}
