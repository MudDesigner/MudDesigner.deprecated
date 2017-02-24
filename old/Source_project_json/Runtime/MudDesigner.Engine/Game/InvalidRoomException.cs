//-----------------------------------------------------------------------
// <copyright file="InvalidRoomException.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine.Game
{
    using System;

    /// <summary>
    /// An exception that will be thrown when a world tries to add an invalid Room
    /// </summary>
    public class InvalidRoomException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidRoomException"/> class.
        /// </summary>
        /// <param name="room">The Room that was invalid.</param>
        /// <param name="message">The message.</param>
        public InvalidRoomException(IRoom room, string message) : base(message)
        {
            this.Data.Add(room, message);
        }
    }
}
