//-----------------------------------------------------------------------
// <copyright file="AdapterBase.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Provodes methods for creating and using a configurable adapter
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration that this adapter supports.</typeparam>
    public abstract class AdapterBase<TConfiguration> : AdapterBase, IAdapter<TConfiguration>, IDisposable where TConfiguration : IConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdapterBase{TConfiguration}"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        protected AdapterBase(TConfiguration configuration) : base(configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdapterBase{TConfiguration}"/> class.
        /// </summary>
        protected AdapterBase()
        {
        }

        /// <summary>
        /// Gets or sets the adapter configuration that will be used to configure this adapter.
        /// </summary>
        public new TConfiguration Configuration { get; protected set; }

        /// <summary>
        /// Configures this adapter using the given configuration.
        /// </summary>
        /// <param name="configuration">The configuration class used by this adapter.</param>
        public abstract void Configure(TConfiguration configuration);

        /// <summary>
        /// Configures this adapter for use within a game.
        /// </summary>
        public override void Configure()
        {
            if (this.Configuration == null)
            {
                return;
            }

            this.Configure(this.Configuration);
        }
    }

    /// <summary>
    /// Provides an interface for creating adapters that the game can start and run
    /// </summary>
    public abstract class AdapterBase : IAdapter, IDisposable
    {
        /// <summary>
        /// The publication subscriptions for this adapter
        /// </summary>
        private Dictionary<Type, ISubscription> subscriptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdapterBase"/> class.
        /// </summary>
        protected AdapterBase()
        {
            this.subscriptions = new Dictionary<Type, ISubscription>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdapterBase"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        protected AdapterBase(IConfiguration configuration) : this()
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the message broker that will be used for publishing messages from this component.
        /// </summary>
        public IMessageBroker MessageBroker { get; set; }

        /// <summary>
        /// Gets or sets the adapter configuration that will be used to configure this adapter.
        /// </summary>
        public IConfiguration Configuration { get; protected set; }

        /// <summary>
        /// Publishes a given message to any subscriber.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="message">The message payload.</param>
        public void PublishMessage<TMessage>(TMessage message) where TMessage : class, IMessage
        {
            if (this.MessageBroker == null)
            {
                this.MessageBroker = MessageBrokerFactory.Instance;
            }

            this.MessageBroker.Publish(message);
        }

        /// <summary>
        /// Subscribes to a specific message.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="callback">The callback delegate that can handle the payload provided.</param>
        /// <param name="predicate">The predicate that governs whether or not the callback is invoked..</param>
        public void SubscribeToMessage<TMessage>(Action<TMessage, ISubscription> callback, Func<TMessage, bool> predicate = null) where TMessage : class, IMessage
        {
            if (this.MessageBroker == null)
            {
                this.MessageBroker = MessageBrokerFactory.Instance;
            }

            ISubscription subscription = null;
            Type messageType = typeof(TMessage);
            if (this.subscriptions.TryGetValue(messageType, out subscription))
            {
                subscription.Unsubscribe();
                this.subscriptions.Remove(messageType);
            }

            subscription = this.MessageBroker.Subscribe<TMessage>(callback, predicate);
            this.subscriptions.Add(messageType, subscription);
        }

        /// <summary>
        /// Unsubscribes from listening to publications of the message specified.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        public void UnsubscribeFromMessage<TMessage>() where TMessage : class, IMessage
        {
            Type messageType = typeof(TMessage);
            ISubscription subscription = null;
            if (!this.subscriptions.TryGetValue(messageType, out subscription))
            {
                return;
            }

            subscription.Unsubscribe();
            this.subscriptions.Remove(messageType);
        }

        /// <summary>
        /// Unsubscribes from all messages.
        /// </summary>
        public void UnsubscribeFromAllMessages()
        {
            foreach (KeyValuePair<Type, ISubscription> pair in this.subscriptions)
            {
                pair.Value.Unsubscribe();
            }

            this.subscriptions.Clear();
        }

        /// <summary>
        /// Sets the notification manager.
        /// </summary>
        /// <param name="broker">The broker.</param>
        public void SetNotificationManager(IMessageBroker broker)
        {
            this.MessageBroker = broker;
        }

        /// <summary>
        /// Configures this adapter for use within a game.
        /// </summary>
        public abstract void Configure();

        /// <summary>
        /// Initializes the component.
        /// </summary>
        /// <returns>
        /// Returns an awaitable Task
        /// </returns>
        public abstract Task Initialize();

        /// <summary>
        /// Starts this adapter and allows it to run.
        /// </summary>
        /// <param name="game">The an instance of an initialized game.</param>
        /// <returns>
        /// Returns an awaitable Task
        /// </returns>
        public abstract Task Start(IGame game);

        /// <summary>
        /// Lets this instance know that it is about to go out of scope and disposed.
        /// The instance will perform clean-up of its resources in preperation for deletion.
        /// </summary>
        /// <returns>
        /// Returns an awaitable Task
        /// </returns>
        /// <para>
        /// Informs the component that it is no longer needed, allowing it to perform clean up.
        /// Objects registered to one of the two delete events will be notified of the delete request.
        /// </para>
        public abstract Task Delete();

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            this.UnsubscribeFromAllMessages();
        }
    }
}
