//-----------------------------------------------------------------------
// <copyright file="TimeOfDayChangedEventArgs.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine.Game
{
    using System;

    /// <summary>
    /// Event arguments for when a time period changes.
    /// </summary>
    public sealed class TimeOfDayChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeOfDayChangedEventArgs"/> class.
        /// </summary>
        /// <param name="transitionFrom">The time period being transitioned from.</param>
        /// <param name="transitionTo">The time period being transition to.</param>
        public TimeOfDayChangedEventArgs(ITimePeriod transitionFrom, ITimePeriod transitionTo)
        {
            if (transitionTo == null)
            {
                throw new ArgumentNullException(nameof(transitionTo), "A state must be provided to transition to.");
            }
            else if (transitionFrom == null)
            {
                throw new ArgumentNullException(nameof(transitionFrom), "A state must be provided to transition from.");
            }

            this.TransitioningFrom = transitionFrom;
            this.TransitioningTo = transitionTo;
        }

        /// <summary>
        /// Gets the state that is being transitioned away from.
        /// </summary>
        public ITimePeriod TransitioningFrom { get; }

        /// <summary>
        /// Gets the state that is being transitioned to.
        /// </summary>
        public ITimePeriod TransitioningTo { get; }
    }
}
