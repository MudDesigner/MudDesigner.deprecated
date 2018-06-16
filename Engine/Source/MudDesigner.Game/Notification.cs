using System;
using System.Threading.Tasks;

namespace MudEngine
{
    class Notification<TMessage> : INotification<TMessage> where TMessage : IMessage
    {
        private Func<TMessage, Task<bool>> asyncCondition;
        private Func<TMessage, ISubscription, Task> asyncCallback;

        /// <summary>
        /// Occurs when the subscription is being unsubscribed.
        /// </summary>
        public event Action<NotificationArgs> Unsubscribing;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ISubscription" /> is active.
        /// </summary>
        public bool IsActive { get; protected set; }

        /// <summary>
        /// Registers a callback for when a chat message is published by the MessageCenter
        /// </summary>
        /// <param name="processor">The message.</param>
        /// <returns></returns>
        public void Register(Func<TMessage, ISubscription, Task> asyncProcessor, Func<TMessage, Task<bool>> asyncCondition)
        {
            this.asyncCallback = asyncProcessor;
            this.asyncCondition = asyncCondition;
            this.IsActive = true;
        }

        /// <summary>
        /// Unsubscribes the handler from notifications. This cleans up all of the callback references and conditions.
        /// </summary>
        public void Unsubscribe()
        {
            this.asyncCallback = null;
            this.asyncCondition = null;

            try
            {
                this.OnUnsubscribing();
            }
            finally
            {
                this.IsActive = false;
            }
        }

        /// <summary>
        /// Processes the message by verifying the callbacks can be invoked, then invoking them.
        /// </summary>
        /// <param name="message">The message.</param>
        public async Task ProcessMessageAsync(TMessage message)
        {
            if (this.asyncCallback == null)
            {
                return;
            }
            
            if (this.asyncCondition != null)
            {
                bool canProcess = await this.asyncCondition(message);
                if (!canProcess)
                {
                    return;
                }
            }

            await this.asyncCallback?.Invoke(message, this);
        }

        /// <summary>
        /// Called when the notification is being unsubscribed from.
        /// </summary>
        protected virtual void OnUnsubscribing()
        {
            var handler = this.Unsubscribing;
            if (handler == null)
            {
                return;
            }

            handler(new NotificationArgs(this, typeof(TMessage)));
        }
    }
}
