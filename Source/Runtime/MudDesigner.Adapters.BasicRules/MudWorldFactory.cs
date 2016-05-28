//-----------------------------------------------------------------------
// <copyright file="MudWorldFactory.cs" company="Sully">
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
    /// 
    /// </summary>
    public sealed class MudWorldFactory : IWorldFactory
    {
        /// <summary>
        /// The realm factory required by the MudWorld constructor
        /// </summary>
        readonly IRealmFactory realmFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="MudWorldFactory"/> class.
        /// </summary>
        /// <param name="realmFactory">A realm factory that can be used for creating realms in the future.</param>
        public MudWorldFactory(IRealmFactory realmFactory)
        {
            this.realmFactory = realmFactory;
        }

        /// <summary>
        /// Creates an uninitialized world.
        /// </summary>
        /// <param name="name">The name of the world.</param>
        /// <param name="gameDayToRealWorldHoursRatio">The game day to real world hours ratio.</param>
        /// <param name="hoursPerDay">The number of hours per day.</param>
        /// <returns>Returns an IWorld instance</returns>
        public Task<IWorld> CreateWorld(string name, double gameDayToRealWorldHoursRatio, int hoursPerDay)
                    => this.CreateWorld(name, gameDayToRealWorldHoursRatio, hoursPerDay, Enumerable.Empty<ITimePeriod>(), Enumerable.Empty<IRealm>());

        /// <summary>
        /// Creates an uninitialized world
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="gameDayToRealWorldHoursRatio">The game day to real world hours ratio.</param>
        /// <param name="hoursPerDay">The number of hours per day.</param>
        /// <param name="timePeriods">The time periods available to the world.</param>
        /// <returns>Returns an IWorld instance</returns>
        public Task<IWorld> CreateWorld(string name, double gameDayToRealWorldHoursRatio, int hoursPerDay, IEnumerable<ITimePeriod> timePeriods)
                    => this.CreateWorld(name, gameDayToRealWorldHoursRatio, hoursPerDay, timePeriods, Enumerable.Empty<IRealm>());

        /// <summary>
        /// Creates an uninitialized world
        /// Each realm provided will be initialized during the worlds initialization.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="gameDayToRealWorldHoursRatio">The game day to real world hours ratio.</param>
        /// <param name="hoursPerDay">The hours per day.</param>
        /// <param name="realms">The realms being added to the world.</param>
        /// <returns>Returns an IWorld instance</returns>
        public Task<IWorld> CreateWorld(string name, double gameDayToRealWorldHoursRatio, int hoursPerDay, IEnumerable<IRealm> realms)
                    => this.CreateWorld(name, gameDayToRealWorldHoursRatio, hoursPerDay, Enumerable.Empty<ITimePeriod>(), realms);

        /// <summary>
        /// Creates an uninitialized world
        /// Each realm provided will be initialized during the worlds initialization.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="gameDayToRealWorldHoursRatio">The game day to real world hours ratio.</param>
        /// <param name="hoursPerDay">The number of hours per day.</param>
        /// <param name="timePeriods">The time periods available to the world.</param>
        /// <param name="realms">The realms being added to the world.</param>
        /// <returns>Returns an IWorld instance</returns>
        public async Task<IWorld> CreateWorld(string name, double gameDayToRealWorldHoursRatio, int hoursPerDay, IEnumerable<ITimePeriod> timePeriods, IEnumerable<IRealm> realms)
        {
            var world = new MudWorld(this.realmFactory, timePeriods);

            world.SetName(name);
            world.GameDayToRealHourRatio = gameDayToRealWorldHoursRatio;
            world.SetHoursPerDay(hoursPerDay);

            if (realms.Count() > 0)
            {
                await world.AddRealmsToWorld(realms);
            }

            return world;
        }
    }
}
