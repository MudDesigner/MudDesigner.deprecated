//-----------------------------------------------------------------------
// <copyright file="MudRoomFactory.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MudDesigner.Engine.Game;

namespace MudDesigner.Adapters.BasicRules
{
    /// <summary>
    /// Provides methods for creating an instance of an IRoom implementation
    /// </summary>
    public sealed class MudRoomFactory : IRoomFactory
    {
        /// <summary>
        /// The doorway factory
        /// </summary>
        readonly IDoorwayFactory doorwayFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="MudRoomFactory"/> class.
        /// </summary>
        /// <param name="doorFactory">The door factory.</param>
        public MudRoomFactory(IDoorwayFactory doorFactory)
        {
            this.doorwayFactory = doorFactory;
        }

        /// <summary>
        /// Creates an uninitialized, sealed room.
        /// </summary>
        /// <param name="name">The name of the room.</param>
        /// <param name="owner">The zone that owns this room.</param>
        /// <returns>Returns an uninitialized room instance</returns>
        public Task<IRoom> CreateRoom(string name, IZone owner)
        {
            var room = new MudRoom(this.doorwayFactory, owner);
            room.SealRoom();

            return Task.FromResult((IRoom)room);
        }
    }
}
