using System;
using System.Threading.Tasks;

namespace MudEngine
{
    /// <summary>
    /// Processes a subscription message.
    /// </summary>
    /// <typeparam name="TMessageType">The type of the message type.</typeparam>
    public interface INotification<TMessageType> : ISubscription where TMessageType : IMessage
    {
        /// <summary>
        /// Registers the specified delegate for callback when a notification is fired for T.
        /// </summary>
        /// <param name="callback">The message being posted along with the subscription registered to receive the post.</param>
        /// <returns></returns>
        void Register(Func<TMessageType, ISubscription, Task> asyncProcessor, Func<TMessageType, Task<bool>> asyncCondition);

        /// <summary>
        /// Processes the message, invoking the registered callbacks if their conditions are met.
        /// </summary>
        /// <param name="message">The message.</param>
        Task ProcessMessageAsync(TMessageType message);
    }
}
