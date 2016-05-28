//-----------------------------------------------------------------------
// <copyright file="MudDoor.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Threading.Tasks;
using MudDesigner.Engine;
using MudDesigner.Engine.Game;

namespace MudDesigner.Adapters.BasicRules
{
    /// <summary>
    /// Provides methods for linking two rooms together via a doorway
    /// </summary>
    public class MudDoor : GameComponent, IDoorway, ICloneableComponent<IDoorway>
    {
        /// <summary>
        /// Gets the room that an IActor would be arriving into during travel.
        /// </summary>
        public IRoom ArrivalRoom { get; private set; }

        /// <summary>
        /// Gets the direction needed to travel in order to leave the DepartureRoom.
        /// </summary>
        public ITravelDirection DepartureDirection { get; private set; }

        /// <summary>
        /// Gets the room that an IActor would be departing from.
        /// </summary>
        public IRoom DepartureRoom { get; private set; }

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
        public Task ConnectRooms(ITravelDirection departureDirection, IRoom departureRoom, IRoom arrivalRoom)
        {
            this.ArrivalRoom = arrivalRoom;
            this.DepartureRoom = departureRoom;
            this.DepartureDirection = departureDirection;

            return Task.FromResult(0);
        }

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
        public async Task ConnectRooms(ITravelDirection departureDirection, IRoom departureRoom, IRoom arrivalRoom, bool autoCreateReverseDoorway)
        {
            this.DepartureDirection = departureDirection;
            this.ArrivalRoom = arrivalRoom;

            if (!autoCreateReverseDoorway)
            {
                return;
            }

            IDoorway oppositeDoorway = await arrivalRoom.CreateDoorway(departureDirection.GetOppositeDirection());
            await oppositeDoorway.ConnectRoom(departureRoom);
            await arrivalRoom.AddDoorwayToRoom(oppositeDoorway);
        }

        /// <summary>
        /// Connects a departing room to a doorawy.
        /// </summary>
        /// <param name="departureDirection">The direction need to travel in order to leave the departure room.</param>
        /// <param name="departureRoom">The room that an IActor would be departing from.</param>
        /// <returns>
        /// Returns an awaitable Task
        /// </returns>
        public Task ConnectRoom(ITravelDirection departureDirection, IRoom departureRoom)
        {
            this.DepartureDirection = departureDirection;
            this.DepartureRoom = departureRoom;

            return Task.FromResult(0);
        }

        /// <summary>
        /// Connects the doorway to an arriving room.
        /// </summary>
        /// <param name="arrivalRoom">The room that an IActor would be arriving in.</param>
        /// <returns>
        /// Returns an awaitable Task
        /// </returns>
        public Task ConnectRoom(IRoom arrivalRoom)
        {
            this.ArrivalRoom = arrivalRoom;

            return Task.FromResult(0);
        }

        /// <summary>
        /// Disconnects the arrival room.
        /// </summary>
        public void DisconnectArrivalRoom()
        {
            if (this.ArrivalRoom == null)
            {
                return;
            }

            this.ArrivalRoom.RemoveDoorwayFromRoom(this.DepartureDirection.GetOppositeDirection());
            this.ArrivalRoom = null;

            // If the doorway has been completely disconnected from both directions, clear the travel direction.
            if (this.DepartureRoom == null)
            {
                this.DepartureDirection = null;
            }
        }

        /// <summary>
        /// Disconnects the departure room from this doorway.
        /// </summary>
        public void DisconnectDepartureRoom()
        {
            if (this.DepartureRoom == null)
            {
                return;
            }

            this.DepartureRoom.RemoveDoorwayFromRoom(this.DepartureDirection);
            this.DepartureRoom = null;

            if (this.ArrivalRoom == null)
            {
                this.DepartureDirection = null;
            }
        }

        /// <summary>
        /// Disconnects the two linked rooms from each other. 
        /// </summary>
        /// <para>
        /// If there are no other rooms connecting the arrival, or the departuring, room then actors can be orphaned
        /// in the room, unable to escape.
        /// </para>
        public void DisconnectRooms()
        {
            if (this.ArrivalRoom != null)
            {
                this.ArrivalRoom.RemoveDoorwayFromRoom(this.DepartureDirection.GetOppositeDirection());
            }

            if (this.DepartureRoom != null)
            {
                this.DepartureRoom.RemoveDoorwayFromRoom(this.DepartureDirection);
            }

            this.ArrivalRoom = null;
            this.DepartureDirection = null;
            this.DepartureRoom = null;
        }

        /// <summary>
        /// Clones the properties of this instance to a new instance.
        /// </summary>
        /// <returns>
        /// Returns a new instance with the properties of this instance copied to it.
        /// </returns>
        /// <para>
        /// Cloning does not guarantee that the internal state of an object will be cloned nor
        /// does it guarantee that the clone will be a deep clone or a shallow.
        /// </para>
        public IDoorway Clone() => new MudDoor
        {
            ArrivalRoom = this.ArrivalRoom,
            DepartureDirection = this.DepartureDirection,
            DepartureRoom = this.DepartureRoom,
            Id = this.Id,
            IsEnabled = this.IsEnabled,
            Name = this.Name,
        };

        /// <summary>
        /// Loads the component and any resources or dependencies it might have.
        /// Called during initialization of the component
        /// </summary>
        /// <returns></returns>
        protected override Task Load() => Task.FromResult(0);

        /// <summary>
        /// Unloads this instance and any resources or dependencies it might be using.
        /// Called during deletion of the component.
        /// </summary>
        /// <returns></returns>
        protected override Task Unload()
        {
            this.DisconnectRooms();
            return Task.FromResult(0);
        }
    }
}
