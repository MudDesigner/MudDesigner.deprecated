//-----------------------------------------------------------------------
// <copyright file="NorthDirection.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using MudDesigner.Engine.Game;

namespace MudDesigner.Adapters.BasicRules
{
    /// <summary>
    /// A direction that represents North.
    /// </summary>
    public struct NorthDirection : ITravelDirection
    {
        /// <summary>
        /// Gets the direction that can be traveled.
        /// </summary>
        public string Direction => "North";

        /// <summary>
        /// Gets the opposite direction associated with this instance.
        /// </summary>
        /// <returns>
        /// Returns the direction required to travel in order to go in the opposite direction of this instance.
        /// </returns>
        public ITravelDirection GetOppositeDirection() => new SouthDirection();
    }
}
