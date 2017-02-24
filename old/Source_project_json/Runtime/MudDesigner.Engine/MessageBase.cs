//-----------------------------------------------------------------------
// <copyright file="MessageBase.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine
{
    /// <summary>
    /// Provides methods for dispatching notifications to subscription handlers
    /// </summary>
    /// <typeparam name="TMessageType">The type of the message type.</typeparam>
    public abstract class MessageBase<TContentType> : IMessage<TContentType> where TContentType : class
    {
        /// <summary>
        /// Gets the content of the message.
        /// </summary>
        public TContentType Content { get; protected set; }

        /// <summary>
        /// Gets the content of the message.
        /// </summar>y
        public TContentType GetContent() => this.Content;

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <returns>
        /// Returns the content of the message
        /// </returns>
        object IMessage.GetContent() => this.GetContent();
    }
}
