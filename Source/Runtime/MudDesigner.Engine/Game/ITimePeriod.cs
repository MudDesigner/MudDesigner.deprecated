//-----------------------------------------------------------------------
// <copyright file="ITimePeriod.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine.Game
{
    using System;

    /// <summary>
    /// Defines a period of time in a given day that automatically increments once initialized
    /// </summary>
    public interface ITimePeriod : IComponent
    {
        /// <summary>
        /// Occurs everytime the TimeOfDay is changed.
        /// </summary>
        event EventHandler<ITimeOfDay> TimeUpdated;

        /// <summary>
        /// Gets the current time of day for this time period.
        /// </summary>
        ITimeOfDay CurrentTime { get; }

        /// <summary>
        /// Gets or sets the name of this time period.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets the start time for this time period. This defines what time of day this period begins at.
        /// </summary>
        ITimeOfDay StateStartTime { get; }

        /// <summary>
        /// Starts the time period, ticking the time of day clock.
        /// </summary>
        /// <param name="startTime">The time of day this period begins at.</param>
        /// <param name="worldTimeFactor">The world time factor. This value can be used to adjust the interval between time of day updates.</param>
        void Start(ITimeOfDay startTime, double worldTimeFactor);

        /// <summary>
        /// Resets this instance, zeroing out the current time back to the time periods start time.
        /// </summary>
        void Reset();
    }
}