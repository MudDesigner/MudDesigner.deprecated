//-----------------------------------------------------------------------
// <copyright file="IDoorway.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine.Game
{
    using System.Threading.Tasks;

    /// <summary>
    /// Provides methods for linking two rooms together via a doorway
    /// </summary>
    public interface IDoorway : IGameComponent
    {
        /// <summary>
        /// Gets the direction needed to travel in order to leave the DepartureRoom.
        /// </summary>
        ITravelDirection DepartureDirection { get; }

        /// <summary>
        /// Gets the room that an IActor would be departing from.
        /// </summary>
        IRoom DepartureRoom { get; }

        /// <summary>
        /// Gets the room that an IActor would be arriving into during travel.
        /// </summary>
        IRoom ArrivalRoom { get; }

        /// <summary>
        /// Connects the departing room to the arrival room.
        /// </summary>
        /// <param name="departureDirection">The departure direction.</param>
        /// <param name="departureRoom">The room that an IActor would be departing from.</param>
        /// <param name="arrivalRoom">The room that an IActor would be arriving into during travel.</param>
        /// <returns>
        /// Returns an awaitable Task
        /// </returns>
        /// <para>
        /// This connection is one-way. Actors can not travel back to the departing room.
        /// </para>
        Task ConnectRooms(ITravelDirection departureDirection, IRoom departureRoom, IRoom arrivalRoom);

        /// <summary>
        /// Connects the departing room to the arrival room, allowing actors to travel between the two rooms.
        /// </summary>
        /// <param name="departureDirection">The departure direction.</param>
        /// <param name="departureRoom">The room that an IActor would be departing from.</param>
        /// <param name="arrivalRoom">The room that an IActor would be arriving into during travel.</param>
        /// <param name="autoCreateReverseDoorway">if set to <c>true</c> a doorway will be added to the arrival room so actors can travel back to the departing room.</param>
        /// <returns>
        /// Returns an awaitable Task
        /// </returns>
        /// <para>
        /// This connection can be set to either one-way or two way, allowing or disallowing
        /// Actors the ability to travel back to the departing room.
        /// </para>
        Task ConnectRooms(ITravelDirection departureDirection, IRoom departureRoom, IRoom arrivalRoom, bool autoCreateReverseDoorway);

        /// <summary>
        /// Connects a departing room to a doorawy.
        /// </summary>
        /// <param name="departureDirection">The direction need to travel in order to leave the departure room.</param>
        /// <param name="departureRoom">The room that an IActor would be departing from.</param>
        /// <returns>
        /// Returns an awaitable Task
        /// </returns>
        Task ConnectRoom(ITravelDirection departureDirection, IRoom departureRoom);

        /// <summary>
        /// Connects the doorway to an arriving room.
        /// </summary>
        /// <param name="arrivalRoom">The room that an IActor would be arriving in.</param>
        /// <returns>
        /// Returns an awaitable Task
        /// </returns>
        Task ConnectRoom(IRoom arrivalRoom);

        /// <summary>
        /// Disconnects the two linked rooms from each other. 
        /// </summary>
        /// <para>
        /// If there are no other rooms connecting the arrival, or the departuring, room then actors can be orphaned
        /// in the room, unable to escape.
        /// </para>
        void DisconnectRooms();

        /// <summary>
        /// Disconnects the arrival room.
        /// </summary>
        void DisconnectArrivalRoom();

        /// <summary>
        /// Disconnects the departure room from this doorway.
        /// </summary>
        void DisconnectDepartureRoom();
    }
}
