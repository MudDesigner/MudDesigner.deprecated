//-----------------------------------------------------------------------
// <copyright file="IComponent.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine
{
    using System;

    /// <summary>
    /// Provides properties for managing the life-cycle of a component
    /// </summary>
    public interface IComponent
    {
        /// <summary>
        /// Gets the unique identifier for this component.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Gets the date that this component was instanced.
        /// </summary>
        DateTime CreationDate { get; }

        /// <summary>
        /// Gets the amount number of seconds that this component instance has been alive.
        /// </summary>
        double TimeAlive { get; }

        /// <summary>
        /// Disables this instance.
        /// </summary>
        void Disable();

        /// <summary>
        /// Enables this instance.
        /// </summary>
        void Enable();
    }
}
