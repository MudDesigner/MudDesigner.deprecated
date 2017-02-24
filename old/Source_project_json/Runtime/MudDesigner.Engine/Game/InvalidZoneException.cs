//-----------------------------------------------------------------------
// <copyright file="InvalidZoneException.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine.Game
{
    using System;

    /// <summary>
    /// An exception that will be thrown when a realm tries to add an invalid zone
    /// </summary>
    public class InvalidZoneException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidZoneException"/> class.
        /// </summary>
        /// <param name="zone">The zone that was invalid.</param>
        /// <param name="message">The message.</param>
        public InvalidZoneException(IZone zone, string message) : base(message)
        {
            this.Data.Add(zone, message);
        }
    }
}
