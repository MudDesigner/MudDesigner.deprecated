//-----------------------------------------------------------------------
// <copyright file="InvalidTimeOfDayException.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine.Game
{
    using System;

    /// <summary>
    /// Thrown when the time of day being created contains invalid time properties
    /// </summary>
    public sealed class InvalidTimeOfDayException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidTimeOfDayException"/> class.
        /// </summary>
        /// <param name="message">The message to apply to the excention.</param>
        /// <param name="timeOfDay">The time of day instance that was invalid, causing the exception.</param>
        public InvalidTimeOfDayException(string message, ITimeOfDay timeOfDay) : base(message)
        {
            this.Data.Add(timeOfDay.GetType(), timeOfDay);
        }
    }
}
