//-----------------------------------------------------------------------
// <copyright file="OccupancyChangedEventArgs.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine.Game
{
    using System;

    public class RoomOccupancyChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoomOccupancyChangedEventArgs" /> class.
        /// </summary>
        /// <param name="occupant">The occupant responsible for the change.</param>
        /// <param name="travelDirection">The direction of travel the occupant is going.</param>
        /// <param name="departureRoom">The room the occupant is departing from.</param>
        /// <param name="arrivalRoom">The room the occupant is arriving in.</param>
        /// <exception cref="System.ArgumentNullException">
        /// A valid Occupant must be provided.
        /// or
        /// A valid travelDirection must be provided.
        /// or
        /// A valid departureRoom must be provided.
        /// or
        /// A valid arrivalRoom must be provided.
        /// </exception>
        public RoomOccupancyChangedEventArgs(ICharacter occupant, ITravelDirection travelDirection, IRoom departureRoom, IRoom arrivalRoom)
        {
            if (occupant == null)
            {
                throw new ArgumentNullException(nameof(occupant), "A valid Occupant must be provided.");
            }
            else if (travelDirection == null)
            {
                throw new ArgumentNullException(nameof(travelDirection), "A valid travelDirection must be provided.");
            }
            else if (departureRoom == null)
            {
                throw new ArgumentNullException(nameof(departureRoom), "A valid departureRoom must be provided.");
            }
            else if (arrivalRoom == null)
            {
                throw new ArgumentNullException(nameof(arrivalRoom), "A valid arrivalRoom must be provided.");
            }

            this.Occupant = occupant;
            this.DepartureRoom = departureRoom;
            this.ArrivalRoom = arrivalRoom;
            this.TravelDirection = travelDirection;
        }

        /// <summary>
        /// Gets the occupant that triggered this event.
        /// </summary>
        public ICharacter Occupant { get; }

        /// <summary>
        /// Gets the direction that the occupant traveled when leaving the departure room.
        /// </summary>
        public ITravelDirection TravelDirection { get; }

        /// <summary>
        /// Gets the room the occupant is departing from.
        /// </summary>
        public IRoom DepartureRoom { get; }

        /// <summary>
        /// Gets the room the occupant is arriving in.
        /// </summary>
        public IRoom ArrivalRoom { get; }
    }
}
