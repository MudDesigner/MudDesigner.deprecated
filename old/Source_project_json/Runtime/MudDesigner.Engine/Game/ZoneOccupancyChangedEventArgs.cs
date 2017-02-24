//-----------------------------------------------------------------------
// <copyright file="ZoneOccupancyChangedEventArgs.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine.Game
{
    public class ZoneOccupancyChangedEventArgs : RoomOccupancyChangedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZoneOccupancyChangedEventArgs"/> class.
        /// </summary>
        /// <param name="zone">The zone.</param>
        /// <param name="roomOccupancyChange">The <see cref="RoomOccupancyChangedEventArgs"/> instance containing the event data.</param>
        public ZoneOccupancyChangedEventArgs(IZone zone, RoomOccupancyChangedEventArgs roomOccupancyChange) 
            : base(roomOccupancyChange.Occupant, roomOccupancyChange.TravelDirection, roomOccupancyChange.DepartureRoom, roomOccupancyChange.ArrivalRoom)
        {
            this.AffectedZone = zone;
        }

        /// <summary>
        /// Gets the affected zone.
        /// </summary>
        public IZone AffectedZone { get; }
    }
}
