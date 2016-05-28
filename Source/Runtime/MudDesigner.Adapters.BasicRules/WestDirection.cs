//-----------------------------------------------------------------------
// <copyright file="WestDirection.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using MudDesigner.Engine.Game;

namespace MudDesigner.Adapters.BasicRules
{
    /// <summary>
    /// A direction that represents West.
    /// </summary>
    public struct WestDirection : ITravelDirection
    {
        /// <summary>
        /// Gets the direction that can be traveled.
        /// </summary>
        public string Direction => "West";

        /// <summary>
        /// Gets the opposite direction associated with this instance.
        /// </summary>
        /// <returns>
        /// Returns the direction required to travel in order to go in the opposite direction of this instance.
        /// </returns>
        public ITravelDirection GetOppositeDirection() => new EastDirection();
    }
}
