//-----------------------------------------------------------------------
// <copyright file="MudRealmFactory.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MudDesigner.Engine.Game;

namespace MudDesigner.Adapters.BasicRules
{

    /// <summary>
    /// Provides methods for creating a new IRealm implementation
    /// </summary>
    public sealed class MudRealmFactory : IRealmFactory
    {
        /// <summary>
        /// The zone factory used by the MudRealm class
        /// </summary>
        readonly IZoneFactory zoneFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="MudRealmFactory"/> class.
        /// </summary>
        /// <param name="zoneFactory">An IZoneFactory implementation required for setting up Realms.</param>
        public MudRealmFactory(IZoneFactory zoneFactory)
        {
            this.zoneFactory = zoneFactory;
        }

        /// <summary>
        /// Creates an unintialized instance of a realm.
        /// </summary>
        /// <param name="name">The name of the realm.</param>
        /// <param name="owner">The world that owns this realm.</param>
        /// <returns>Returns an unintialized instance of IRealm</returns>
        public Task<IRealm> CreateRealm(string name, IWorld owner)
                    => this.CreateRealm(name, owner, null, Enumerable.Empty<IZone>());

        /// <summary>
        /// Creates an unintialized instance of a realm.
        /// All of the children zones will be initialized prior to being added to the realm.
        /// </summary>
        /// <param name="name">The name of the realm.</param>
        /// <param name="owner">The world that owns this realm.</param>
        /// <param name="zones">A collection of zones that will be initialized and added to the realm.</param>
        /// <returns>Returns an unintialized instance of IRealm</returns>
        public Task<IRealm> CreateRealm(string name, IWorld owner, IEnumerable<IZone> zones)
                    => this.CreateRealm(name, owner, null, zones);

        /// <summary>
        /// Creates an unintialized instance of a realm.
        /// </summary>
        /// <param name="name">The name of the realm.</param>
        /// <param name="owner">The world that owns this realm.</param>
        /// <param name="timeZoneOffset">The time zone offset to apply to the realm.</param>
        /// <returns>Returns an unintialized instance of IRealm</returns>
        public Task<IRealm> CreateRealm(string name, IWorld owner, ITimeOfDay timeZoneOffset)
                    => this.CreateRealm(name, owner, timeZoneOffset, Enumerable.Empty<IZone>());

        /// <summary>
        /// Creates an unintialized instance of a realm.
        /// All of the children zones will be initialized prior to being added to the realm.
        /// </summary>
        /// <param name="name">The name of the realm.</param>
        /// <param name="owner">The world that owns this realm.</param>
        /// <param name="timeZoneOffset">The time zone offset to apply to the realm.</param>
        /// <param name="zones">A collection of zones that will be initialized and added to the realm.</param>
        /// <returns>Returns an unintialized instance of IRealm</returns>
        public async Task<IRealm> CreateRealm(string name, IWorld owner, ITimeOfDay timeZoneOffset, IEnumerable<IZone> zones)
        {
            var realm = new MudRealm(this.zoneFactory, owner);

            if (timeZoneOffset != null)
            {
                realm.ApplyTimeZoneOffset(timeZoneOffset.Hour, timeZoneOffset.Minute);
            }

            realm.SetName(name);
            if (zones.Count() > 0)
            {
                await realm.AddZonesToRealm(zones);
            }

            return realm;
        }
    }
}
