//-----------------------------------------------------------------------
// <copyright file="IDoorwayFactory.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine.Game
{
    using System.Threading.Tasks;

    /// <summary>
    /// Provides methods for creating an instance of an IDoorway implementation.
    /// </summary>
    public interface IDoorwayFactory
    {
        /// <summary>
        /// Creates an uninitialized instance of a doorway, connected to a departing room.
        /// </summary>
        /// <param name="departureRoom">The room that an IActor would be departing from.</param>
        /// <param name="travelDirection">The direction need to travel in order to leave the departure room.</param>
        /// <returns>Returns an uninitialized doorway</returns>
        Task<IDoorway> CreateDoor(string doorwayName, IRoom departureRoom, ITravelDirection travelDirection);

        /// <summary>
        /// Creates an uninitialized instance of a doorway, connected to a departing and arrival room.
        /// A doorway will be created for the departure room only, making it a one-way link
        /// </summary>
        /// <param name="arrivalRoom">The room that an IActor would be arriving into during travel.</param>
        /// <param name="departureRoom">The room that an IActor would be departing from.</param>
        /// <param name="travelDirection">The direction need to travel in order to leave the departure room.</param>
        /// <returns>Returns an uninitialized doorway</returns>
        Task<IDoorway> CreateDoor(string doorwayName, IRoom arrivalRoom, IRoom departureRoom, ITravelDirection travelDirection);

        /// <summary>
        /// Creates an uninitialized instance of a doorway, connected to a departing and arrival room.
        /// A doorway will be created for both rooms, linking them together from both ends
        /// </summary>
        /// <param name="arrivalRoom">The room that an IActor would be arriving into during travel.</param>
        /// <param name="departureRoom">The room that an IActor would be departing from.</param>
        /// <param name="travelDirection">The direction need to travel in order to leave the departure room.</param>
        /// <returns>Returns an uninitialized doorway</returns>
        Task<IDoorway> CreateTwoWayDoor(string doorwayName, IRoom arrivalRoom, IRoom departureRoom, ITravelDirection travelDirection);
    }
}
