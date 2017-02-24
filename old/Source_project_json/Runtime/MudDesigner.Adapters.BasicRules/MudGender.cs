using System;
using MudDesigner.Engine.Game;

namespace MudDesigner.Adapters.BasicRules
{
    public class MudGender : IGender
    {
        /// <summary>
        /// Gets the date that this component was instanced.
        /// </summary>
        public DateTime CreationDate { get; private set; }

        /// <summary>
        /// Gets or sets a description of the object.
        /// </summary>
        public string Description { get; set;}

        /// <summary>
        /// Gets the unique identifier for this component.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is enabled.
        /// </summary>
        public bool IsEnabled { get; private set; }

        /// <summary>
        /// Gets the name of the object.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the amount number of seconds that this component instance has been alive.
        /// </summary>
        public double TimeAlive => this.CreationDate.Subtract(DateTime.Now).TotalSeconds;

        /// <summary>
        /// Disables this instance.
        /// </summary>
        public void Disable()
        {
            this.IsEnabled = false;
        }

        /// <summary>
        /// Enables this instance.
        /// </summary>
        public void Enable()
        {
            this.IsEnabled = true;
        }

        /// <summary>
        /// Sets the name for the gender.
        /// </summary>
        /// <param name="name">The name.</param>
        public void SetName(string name)
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new InvalidOperationException("You can not provide an empty or null name.");
            }

            this.Name = name;
        }
    }
}
