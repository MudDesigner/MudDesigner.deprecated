using System;

namespace MudDesigner.Engine
{
    public abstract class MudComponent : IComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MudComponent"/> class.
        /// </summary>
        protected MudComponent()
        {
            this.Id = Guid.NewGuid();
            this.CreationDate = DateTime.Now;
        }

        /// <summary>
        /// Gets the unique identifier for this component.
        /// </summary>
        public Guid Id { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether this instance is enabled.
        /// </summary>
        public bool IsEnabled { get; protected set; }

        /// <summary>
        /// Gets the date that this component was instanced.
        /// </summary>
        public DateTime CreationDate { get; }

        /// <summary>
        /// Gets the amount number of seconds that this component instance has been alive.
        /// </summary>
        public double TimeAlive => DateTime.Now.Subtract(this.CreationDate).TotalSeconds;

        /// <summary>
        /// Disables this instance.
        /// </summary>
        public virtual void Disable() => this.IsEnabled = false;

        /// <summary>
        /// Enables this instance.
        /// </summary>
        public virtual void Enable() => this.IsEnabled = true;
    }
}
