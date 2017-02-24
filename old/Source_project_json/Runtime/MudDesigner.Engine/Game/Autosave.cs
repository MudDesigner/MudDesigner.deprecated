//-----------------------------------------------------------------------
// <copyright file="Autosave.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine.Game
{
    using System;
    using System.Threading.Tasks;

    public sealed class Autosave<TComponent> : IDisposable, IInitializableComponent where TComponent : class, IComponent
    {
        /// <summary>
        /// The autosave timer
        /// </summary>
        EngineTimer<TComponent> autosaveTimer;

        /// <summary>
        /// The item to save when the timer fires
        /// </summary>
        TComponent ItemToSave;

        /// <summary>
        /// The delegate to call when the timer fires
        /// </summary>
        Func<Task> saveDelegate;

        /// <summary>
        /// Determines if the autosave instance is disposed of.
        /// </summary>
        bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="Autosave{T}"/> class.
        /// </summary>
        /// <param name="itemToSave">The item to save.</param>
        /// <param name="saveDelegate">The save delegate.</param>
        public Autosave(TComponent itemToSave, Func<Task> saveDelegate)
        {
            if (itemToSave == null)
            {
                throw new ArgumentNullException(nameof(itemToSave), "Can not save a null item.");
            }
            else if (saveDelegate == null)
            {
                throw new ArgumentNullException(nameof(saveDelegate), "Save delegate must not be null.");
            }

            this.ItemToSave = itemToSave;
            this.saveDelegate = saveDelegate;
            this.AutoSaveFrequency = 0;
        }

        /// <summary>
        /// Gets or sets the automatic save frequency in seconds.
        /// Set the frequency to zero in order to disable auto-save.
        /// </summary>
        public int AutoSaveFrequency { get; set; }

        /// <summary>
        /// Gets a value indicating whether the autosave timer is running.
        /// </summary>
        public bool IsAutosaveRunning => this.autosaveTimer != null && this.autosaveTimer.IsRunning;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <returns>Returns an awaitable Task</returns>
        public Task Initialize()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(typeof(Autosave<TComponent>).Name, "Can not initialize the autosave as it has already been disposed of.");
            }

            // Default to saving every 60 seconds if the frequency is under 1 second.
            if (this.AutoSaveFrequency < 1)
            {
                this.AutoSaveFrequency = 60;
            }

            this.autosaveTimer = new EngineTimer<TComponent>(this.ItemToSave);
            double autosaveInterval = TimeSpan.FromSeconds(this.AutoSaveFrequency).TotalMilliseconds;

            this.autosaveTimer.StartAsync(
                autosaveInterval,
                autosaveInterval,
                0,
                async (game, timer) => 
                {
                    await this.saveDelegate();
                    MessageBrokerFactory.Instance.Publish(new AutosaveMessage<TComponent>(this.ItemToSave));
                });

            return Task.FromResult(true);
        }

        /// <summary>
        /// Lets this instance know that it is about to go out of scope and disposed.
        /// The instance will perform clean-up of its resources in preperation for deletion.
        /// </summary>
        /// <returns>Returns an awaitable Task</returns>
        public Task Delete()
        {
            this.autosaveTimer.Stop();
            return Task.FromResult(true);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Delete();
            this.autosaveTimer.Dispose();
            this.isDisposed = true;
        }
    }
}
