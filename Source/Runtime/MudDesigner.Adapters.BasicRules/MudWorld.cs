//-----------------------------------------------------------------------
// <copyright file="MudWorld.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MudDesigner.Engine.Game;

namespace MudDesigner.Adapters.BasicRules
{
    /// <summary>
    /// Provides methods for creating and maintaining worlds
    /// </summary>
    public class MudWorld : GameComponent, IWorld
    {
        /// <summary>
        /// The time periods assigned to this world
        /// </summary>
        List<ITimePeriod> timePeriods;

        /// <summary>
        /// The realms assigned to this world
        /// </summary>
        List<IRealm> realms;

        /// <summary>
        /// The realm factory used to create new realms
        /// </summary>
        IRealmFactory realmFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="MudWorld" /> class.
        /// </summary>
        /// <param name="realmFactory">The realm factory used to create new realms.</param>
        public MudWorld(IRealmFactory realmFactory)
        {
            this.HoursPerDay = 24;
            this.GameDayToRealHourRatio = 0.75;

            this.realmFactory = realmFactory;

            this.realms = new List<IRealm>();
            this.timePeriods = new List<ITimePeriod>();
            this.TimePeriodManager = new TimePeriodManager(Enumerable.Empty<ITimePeriod>());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MudWorld"/> class.
        /// </summary>
        /// <param name="timePeriodsForWorld">The time periods for world.</param>
        public MudWorld(IRealmFactory realmFactory, IEnumerable<ITimePeriod> timePeriodsForWorld) : this(realmFactory)
        {
            this.timePeriods = new List<ITimePeriod>(timePeriodsForWorld);
            this.TimePeriodManager = new TimePeriodManager(timePeriodsForWorld);
        }

        /// <summary>
        /// Occurs when the time of day has changed for this world.
        /// </summary>
        public event EventHandler<TimeOfDayChangedEventArgs> TimeOfDayChanged;

        /// <summary>
        /// Gets the time period manager used to manage the different times of the day a world can be in.
        /// </summary>
        public TimePeriodManager TimePeriodManager { get; private set; }

        /// <summary>
        /// Gets the current time of day period for this world.
        /// </summary>
        public ITimePeriod CurrentTimeOfDay { get; protected set; }

        /// <summary>
        /// Gets or sets the game day to real hour ratio.
        /// </summary>
        public double GameDayToRealHourRatio { get; set; }

        /// <summary>
        /// Gets the adjustment factor used to convert real-world time to in-game time
        /// </summary>
        public double GameTimeAdjustmentFactor => this.GameDayToRealHourRatio / this.HoursPerDay;

        /// <summary>
        /// Gets how many hours it takes to complete one full day in this world.
        /// </summary>
        public int HoursPerDay { get; protected set; }

        /// <summary>
        /// Gets the available time periods for this world.
        /// </summary>
        /// <returns>Returns an array of time periods</returns>
        public ITimePeriod[] GetTimePeriodsForWorld() => this.timePeriods.ToArray();

        /// <summary>
        /// Gets all of the realms that have been added to this world.
        /// </summary>
        /// <returns>Returns an array of realms</returns>
        public IRealm[] GetRealmsInWorld() => this.realms.ToArray();

        /// <summary>
        /// Creates an uninitialized instance of a realm.
        /// </summary>
        /// <param name="name">The name of the realm.</param>
        /// <returns>Returns an uninitialized instance of IRealm</returns>
        public Task<IRealm> CreateRealm(string name) => this.realmFactory.CreateRealm(name, this);

        /// <summary>
        /// Adds a collection of realms to world, initializing them as they are added.
        /// </summary>
        /// <param name="realms">The realms.</param>
        /// <returns>
        /// Returns an awaitable Task
        /// </returns>
        public async Task AddRealmsToWorld(IEnumerable<IRealm> realms)
        {
            if (realms == null)
            {
                return;
            }

            foreach(IRealm realm in realms)
            {
                await this.AddRealmToWorld(realm);
            }
        }

        /// <summary>
        /// Initializes and then adds the given realm to this world instance.
        /// </summary>
        /// <param name="realm">The realm to add.</param>
        /// <returns>Returns an awaitable Task</returns>
        /// <exception cref="MudDesigner.MudEngine.Environment.InvalidRealmException">The realm name can not be null or blank.</exception>
        public async Task AddRealmToWorld(IRealm realm)
        {
            if (this.realms.Contains(realm))
            {
                return;
            }

            if (string.IsNullOrEmpty(realm.Name))
            {
                throw new InvalidRealmException(realm, "The realm name can not be null or blank.");
            }

            realm.Owner = this;
            await realm.Initialize();
            this.realms.Add(realm);
        }

        /// <summary>
        /// Adds the time period to world.
        /// </summary>
        /// <param name="timePeriod">The time period.</param>
        /// <exception cref="MudDesigner.MudEngine.Environment.InvalidTimePeriodException">
        /// The time period must be given a name prior to adding it to the world.
        /// or
        /// The time period must have a starting state time.
        /// </exception>
        /// <exception cref="InvalidTimeOfDayException">The time of day does not define the number of hours there are in a day.</exception>
        public void AddTimePeriodToWorld(ITimePeriod timePeriod)
        {
            if (this.timePeriods.Contains(timePeriod))
            {
                return;
            }

            if (string.IsNullOrEmpty(timePeriod.Name))
            {
                throw new InvalidTimePeriodException(timePeriod, "The time period must be given a name prior to adding it to the world.");
            }
            else if (timePeriod.StateStartTime == null)
            {
                throw new InvalidTimePeriodException(timePeriod, "The time period must have a starting state time.");
            }
            else if (timePeriod.StateStartTime.HoursPerDay == 0)
            {
                throw new InvalidTimeOfDayException("The time of day does not define the number of hours there are in a day.", timePeriod.StateStartTime);
            }
            else if (this.timePeriods.Any(period => period.StateStartTime.Equals(timePeriod.StateStartTime)))
            {
                throw new InvalidTimePeriodException(timePeriod, "You can not have two time periods with the same start time in the same world.");
            }
            
            this.timePeriods.Add(timePeriod);
        }

        /// <summary>
        /// Removes the given realm from this world instance, deleting the realm in the process.
        /// If it must be reused, you may clone the realm and add the clone to another world.
        /// </summary>
        /// <param name="realm">The realm to remove.</param>
        /// <returns>Returns an awaitable Task</returns>
        public Task RemoveRealmFromWorld(IRealm realm)
        {
            if (realm == null)
            {
                return Task.FromResult(0);
            }

            if (!this.realms.Contains(realm))
            {
                return Task.FromResult(0);
            }

            this.realms.Remove(realm);
            return realm.Delete();
        }

        /// <summary>
        /// Removes a collection of realms from this world instance.
        /// If any of the realms don't exist in the world, they will be ignored.
        /// The realms will be deleted during the process.
        /// If they must be reused, you may clone the realm and add the clone to another world.
        /// </summary>
        /// <param name="realms">The realms collection.</param>
        /// <returns>Returns an awaitable Task</returns>
        public async Task RemoveRealmsFromWorld(IEnumerable<IRealm> realms)
        {
            if (realms == null)
            {
                return;
            }

            foreach(IRealm realm in realms)
            {
                await this.RemoveRealmFromWorld(realm);
            }
        }

        /// <summary>
        /// Removes a time period from the world.
        /// </summary>
        /// <param name="timePeriod">The time period being removed.</param>
        public void RemoveTimePeriodFromWorld(ITimePeriod timePeriod)
        {
            if (timePeriod == null)
            {
                return;
            }

            if (this.timePeriods.Contains(timePeriod))
            {
                this.timePeriods.Remove(timePeriod);
            }
        }

        /// <summary>
        /// Sets how many hours it takes the world to complete a full day.
        /// </summary>
        /// <param name="hours">The number of hours that defines how long a full day is.</param>
        public void SetHoursPerDay(int hours)
        {
            this.HoursPerDay = hours;
            foreach(ITimePeriod period in this.timePeriods)
            {
                period.StateStartTime.SetHoursPerDay(hours);
            }
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
        public IWorld Clone()
        {
            var clone = new MudWorld(this.realmFactory)
            {
                Id = this.Id,
                CurrentTimeOfDay = this.CurrentTimeOfDay,
                GameDayToRealHourRatio = this.GameDayToRealHourRatio,
                HoursPerDay = this.HoursPerDay,
                IsEnabled = this.IsEnabled,
                Name = this.Name,
                realms = this.realms,
                timePeriods = this.timePeriods,
            };

            clone.TimePeriodManager = new TimePeriodManager(this.timePeriods);
            return clone;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => string.Format("{0} in the {1} in {2}.", this.CurrentTimeOfDay.CurrentTime.ToString(), this.CurrentTimeOfDay.Name, this.Name);

        /// <summary>
        /// Loads the component and any resources or dependencies it might have.
        /// Called during initialization of the component
        /// </summary>
        /// <returns></returns>
        protected override Task Load()
        {
            // Find a TimePeriod that matches to the current real-world time.
            // If none is found, we grab the first time period in the collection.
            ITimePeriod realWorldTimePeriod = this.timePeriods.FirstOrDefault(
                period => period.CurrentTime.Hour == DateTime.Now.Hour);
            if (realWorldTimePeriod == null)
            {
                realWorldTimePeriod = this.timePeriods.FirstOrDefault();
                if (realWorldTimePeriod == null)
                {
                    throw new InvalidOperationException("Unable to load a world without at least one time period.");
                }
            }

            ITimeOfDay currentTimeOfDay = realWorldTimePeriod.CurrentTime;
            this.CurrentTimeOfDay = this.TimePeriodManager.GetTimePeriodForDay(currentTimeOfDay);

            return Task.FromResult(0);
        }

        /// <summary>
        /// Unloads this instance and any resources or dependencies it might be using.
        /// Called during deletion of the component.
        /// </summary>
        /// <returns></returns>
        protected override Task Unload() => Task.FromResult(0);
    }
}
