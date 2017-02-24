//-----------------------------------------------------------------------
// <copyright file="NotificationArgs.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine
{
    using System;

    /// <summary>
    /// Provides subscribers with the message payload from a publication along with its subscription.
    /// </summary>
    public sealed class NotificationArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationArgs"/> class.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        /// <param name="messageType">Type of the message.</param>
        public NotificationArgs(ISubscription subscription, Type messageType)
        {
            this.Subscription = subscription;
            this.MessageType = messageType;
        }

        /// <summary>
        /// Gets the subscription, allowing for delegates to unsubscribe from future publications if needed.
        /// </summary>
        public ISubscription Subscription { get; }

        /// <summary>
        /// Gets the message payload.
        /// </summary>
        public Type MessageType { get; }
    }
}
