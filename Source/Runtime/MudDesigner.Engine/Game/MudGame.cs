//-----------------------------------------------------------------------
// <copyright file="MudGame.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine.Game
{
    using System;
    using System.Threading.Tasks;
    using System.Threading;

    /// <summary>
    /// The Default engine implementation of the IGame interface. This implementation provides validation support via ValidationBase.
    /// </summary>
    public class MudGame : GameComponent, IGame
    {
        /// <summary>
        /// The adapters that have already been initialized and are ready to be started.
        /// </summary>
        IAdapter[] configuredAdapters;

        /// <summary>
        /// Gets information pertaining to the game.
        /// </summary>
        public IGameConfiguration Configuration { get; protected set; }

        /// <summary>
        /// Gets a value indicating that the initialized or not.
        /// </summary>
        public bool IsRunning { get; protected set; }

        /// <summary>
        /// Gets or sets the last saved.
        /// </summary>
        public DateTime LastSaved { get; }

        /// <summary>
        /// Configures the game using the provided game configuration.
        /// </summary>
        /// <param name="configuration">The configuration the game should use.</param>
        /// <returns>Returns an awaitable Task</returns>
        public Task Configure(IGameConfiguration configuration)
        {
            this.Configuration = configuration;
            this.configuredAdapters = configuration.GetAdapters();
            foreach(IAdapter adapter in this.configuredAdapters)
            {
                adapter.Configure();
            }

            return Task.FromResult(0);
        }

        /// <summary>
        /// Starts the game using the begin/end async pattern. This method requires the caller to handle the process life-cycle management as a loop is not generated internally.
        /// </summary>
        /// <param name="startCompletedCallback">The delegate to invoke when the game startup has completed.</param>
        public void BeginStart(Action<IGame> startCompletedCallback)
        {
            var threadContext = new ThreadContext<IGame>(SynchronizationContext.Current, startCompletedCallback);
            Task.Run(() => this.Start(threadContext));
        }

        /// <summary>
        /// Starts game asynchronously. This will start a game loop that can be awaited. The loop will run until stopped.
        /// </summary>
        /// <returns>Returns an awaitable Task</returns>
        public async Task StartAsync() => await this.Start();

        async Task Start(ThreadContext<IGame> startupContext = null)
        {
            MessageBrokerFactory.Instance.Publish(new GameMessage("Starting game."));

            if (!this.IsEnabled)
            {
                await this.Initialize();
            }

            MessageBrokerFactory.Instance.Publish(new GameMessage("Configuring game configuration components."));
            foreach (IAdapter adapter in this.configuredAdapters)
            {
                MessageBrokerFactory.Instance.Publish(new GameMessage($"Initializing {adapter.Name} component."));
                await adapter.Start(this);
                MessageBrokerFactory.Instance.Publish(new GameMessage($"{adapter.Name} initialization completed."));
            }

            this.IsRunning = true;
            MessageBrokerFactory.Instance.Publish(new GameMessage("Game started."));

            if (startupContext != null)
            {
                startupContext.Invoke(this);
                return;
            }

            // Start the game loop.
            await Task.Run(() =>
            {
                while (this.IsRunning)
                {
                    Task.Delay(1).Wait();
                }
            });
        }

        void notifyStartCompleted(object state)
        {
            var callback = (Action<IGame>)state;
            callback(this);
        }

        /// <summary>
        /// Stops the game from running.
        /// </summary>
        /// <returns>Returns an awaitable Task</returns>
        public async Task Stop()
        {
            this.IsEnabled = false;
            this.IsRunning = false;

            foreach (IAdapter adapter in this.configuredAdapters)
            {
                await adapter.Delete();
            }
        }

        /// <summary>
        /// Loads the component and any resources or dependencies it might have.
        /// Called during initialization of the component
        /// </summary>
        /// <returns></returns>
        protected override async Task Load()
        {
            // Initialize all of our adapters
            this.configuredAdapters = this.Configuration.GetAdapters();
            foreach (IAdapter adapter in this.configuredAdapters)
            {
                await adapter.Initialize();
            }

            this.IsEnabled = true;
        }

        /// <summary>
        /// Unloads this instance and any resources or dependencies it might be using.
        /// Called during deletion of the component.
        /// </summary>
        /// <returns></returns>
        protected override Task Unload()
        {
            if (this.IsRunning)
            {
                return this.Stop();
            }

            return Task.FromResult(0);
        }
    }
}
