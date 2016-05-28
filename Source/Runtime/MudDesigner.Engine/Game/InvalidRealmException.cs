using System;
//-----------------------------------------------------------------------
// <copyright file="InvalidRealmException.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine.Game
{
    /// <summary>
    /// An exception that will be thrown when a world tries to add an invalid realm
    /// </summary>
    public class InvalidRealmException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidRealmException"/> class.
        /// </summary>
        /// <param name="realm">The realm that was invalid.</param>
        /// <param name="message">The message.</param>
        public InvalidRealmException(IRealm realm, string message) : base(message)
        {
            this.Data.Add(realm, message);
        }
    }
}
