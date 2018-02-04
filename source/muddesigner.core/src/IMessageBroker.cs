using System;
using System.Threading.Tasks;

namespace MudEngine
{
    public interface IMessageBroker
    {
        int ActiveSubscriptions { get; }

        /// <summary>
        /// Sets up a new handler and returns it for subscription set up.
        /// </summary>
        /// <typeparam name="TMessageType">An IMessage implementation that the given handler will be provided when messages are dispatched</typeparam>
        /// <param name="handler">The handler used to process incoming messages.</param>
        /// <returns>Returns an ISubscription that can be used to unsubscribe.</returns>
        ISubscription Subscribe<TMessageType>(Func<TMessageType, ISubscription, Task> asyncCallback, Func<TMessageType, Task<bool>> asyncCondition = null) where TMessageType : IMessage;

        /// <summary>
        /// Publishes the specified message.
        /// </summary>
        /// <typeparam name="TMessageType"></typeparam>
        /// <param name="message">The message.</param>
        Task PublishAsync<TMessageType>(TMessageType message) where TMessageType : IMessage;
    }
}