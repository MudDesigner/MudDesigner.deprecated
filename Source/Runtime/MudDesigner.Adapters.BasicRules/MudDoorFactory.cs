//-----------------------------------------------------------------------
// <copyright file="MudDoorFactory.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Threading.Tasks;
using MudDesigner.Engine.Game;

namespace MudDesigner.Adapters.BasicRules
{

    public sealed class MudDoorFactory : IDoorwayFactory
    {
        public async Task<IDoorway> CreateDoor(string doorwayName, IRoom departureRoom, ITravelDirection travelDirection)
        {
            var doorway = new MudDoor();
            await doorway.ConnectRoom(travelDirection, departureRoom);
            doorway.SetName(doorwayName);

            return doorway;
        }

        public async Task<IDoorway> CreateDoor(string doorwayName, IRoom arrivalRoom, IRoom departureRoom, ITravelDirection travelDirection)
        {
            var doorway = new MudDoor();
            await doorway.ConnectRooms(travelDirection, departureRoom, arrivalRoom);
            return doorway;
        }

        /// <summary>
        /// Creates an uninitialized instance of a doorway, connected to a departing and arrival room.
        /// A doorway will be created for both rooms, linking them together from both ends
        /// </summary>
        /// <param name="arrivalRoom">The room that an IActor would be arriving into during travel.</param>
        /// <param name="departureRoom">The room that an IActor would be departing from.</param>
        /// <param name="travelDirection">The direction need to travel in order to leave the departure room.</param>
        /// <returns>Returns an uninitialized doorway</returns>
        public async Task<IDoorway> CreateTwoWayDoor(string doorwayName, IRoom arrivalRoom, IRoom departureRoom, ITravelDirection travelDirection)
        {
            var doorway = new MudDoor();
            await doorway.ConnectRooms(travelDirection, departureRoom, arrivalRoom, true);
            return doorway;
        }
    }
}
