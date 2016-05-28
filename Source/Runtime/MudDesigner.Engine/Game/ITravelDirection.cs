//-----------------------------------------------------------------------
// <copyright file="ITravelDirection.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine.Game
{
    /// <summary>
    /// Represents a direction of travel. Implement this on a class that must define a travel direction.
    /// </summary>
    public interface ITravelDirection
    {
        /// <summary>
        /// Gets the direction that can be traveled.
        /// </summary>
        string Direction { get; }

        /// <summary>
        /// Gets the opposite direction associated with this instance.
        /// </summary>
        /// <returns>
        /// Returns the direction required to travel in order to go in the opposite direction of this instance.
        /// </returns>
        ITravelDirection GetOppositeDirection();
    }
}
