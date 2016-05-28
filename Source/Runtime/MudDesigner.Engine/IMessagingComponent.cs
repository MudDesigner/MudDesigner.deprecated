//-----------------------------------------------------------------------
// <copyright file="MudGame.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine
{
    using System;

    /// <summary>
    /// Provides methods that allow configuration components to subscribe and publish messages to other components
    /// without knowing what other components are loaded at runtime
    /// </summary>
    /// <remarks>
    /// If no broker is provided, the component should use the singleton broker from the MessageBrokerFactory.
    /// </remarks>
    public interface IMessagingComponent
    {
        /// <summary>
        /// Gets the message broker that will be used for publishing messages from this component.
        /// </summary>
        IMessageBroker MessageBroker { get; }

        /// <summary>
        /// Publishes a given message to any subscriber.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="message">The message payload.</param>
        void PublishMessage<TMessage>(TMessage message) where TMessage : class, IMessage;

        /// <summary>
        /// Subscribes to a specific message.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="callback">The callback delegate that can handle the payload provided.</param>
        /// <param name="predicate">The predicate that governs whether or not the callback is invoked..</param>
        void SubscribeToMessage<TMessage>(Action<TMessage, ISubscription> callback, Func<TMessage, bool> predicate = null) where TMessage : class, IMessage;

        /// <summary>
        /// Unsubscribes from listening to publications of the message specified.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        void UnsubscribeFromMessage<TMessage>() where TMessage : class, IMessage;

        /// <summary>
        /// Unsubscribes from all messages.
        /// </summary>
        void UnsubscribeFromAllMessages();

        /// <summary>
        /// Sets the notification manager to be used by this component.
        /// </summary>
        /// <param name="notificationManager">The notification manager.</param>
        void SetNotificationManager(IMessageBroker notificationManager);
    }
}
