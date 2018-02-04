using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MudEngine
{
    public class MessageBroker : IMessageBroker
    {
        private readonly object subscriberLock = new Object();

        /// <summary>
        /// Collection of subscribed listeners
        /// </summary>
        private readonly ConcurrentDictionary<Type, List<ISubscription>> listeners =
            new ConcurrentDictionary<Type, List<ISubscription>>();

        public int ActiveSubscriptions => this.listeners.Sum(subscribers => subscribers.Value.Count);

        public ISubscription Subscribe<TMessageType>(Func<TMessageType, ISubscription, Task> asyncCallback, Func<TMessageType, Task<bool>> asyncCondition = null) where TMessageType : IMessage
        {
            if (asyncCallback == null)
            {
                throw new ArgumentNullException(nameof(asyncCallback), "Callback must not be null when subscribing");
            }

            Type messageType = typeof(TMessageType);

            // Create our key if it doesn't exist along with an empty collection as the value.
            List<ISubscription> subscribers = this.listeners.GetOrAdd(messageType, new List<ISubscription>());

            // Add our notification to our listener collection so we can publish to it later, then return it.
            // TODO: Move instancing the Notification in to a Factory.
            var handler = new Notification<TMessageType>();
            handler.Register(asyncCallback, asyncCondition);
            handler.Unsubscribing += this.Unsubscribe;

            lock (subscriberLock)
            {
                subscribers.Add(handler);
            }

            return handler;
        }

        public async Task PublishAsync<T>(T message) where T : IMessage
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "You can not publish a null message.");
            }

            if (this.listeners.TryGetValue(typeof(T), out List<ISubscription> listenersToPublishTo))
            {
                return;
            }

            // Create a local reference of the collection to protect us against the collection
            // adding a new subscriber while we're enumerating
            foreach (INotification<T> handler in listenersToPublishTo)
            {
                await handler.ProcessMessageAsync(message);
            }
        }

        /// <summary>
        /// Unsubscribes the specified handler by removing their handler from our collection.
        /// </summary>
        /// <typeparam name="T">The message Type you want to unsubscribe from</typeparam>
        /// <param name="subscription">The subscription to unsubscribe.</param>
        void Unsubscribe(NotificationArgs args)
        {
            // If the key doesn't exist or has an empty collection we just return.
            // We will leave the key in there for future subscriptions to use.
            if (!listeners.ContainsKey(args.MessageType) || listeners[args.MessageType].Count == 0)
            {
                return;
            }

            // Remove the subscription from the collection associated with the key.
            List<ISubscription> subscribers = listeners[args.MessageType];
            lock (subscribers)
            {
                subscribers.Remove(args.Subscription);
            }

            args.Subscription.Unsubscribing -= this.Unsubscribe;
        }
    }
}
