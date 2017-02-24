//-----------------------------------------------------------------------
// <copyright file="TimePeriodManager.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine.Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides methods for fetching an ITimePeriod implementation based on a TimeOfDay instance.
    /// </summary>
    public sealed class TimePeriodManager
    {
        // TODO: Replace with a real ITimeOfDayFactory
        /// <summary>
        /// A factory delegate that can be used to create a new instance of an ITimeOfDay component
        /// </summary>
        static Func<double, double, int, ITimeOfDay> _factory;

        /// <summary>
        /// The number of hours per day that the manager will assign to all new ITimePeriod instances
        /// </summary>
        static int _hoursPerDay;

        /// <summary>
        /// The time of day states provided by an external source
        /// </summary>
        readonly List<ITimePeriod> timeOfDayStates;

        /// <summary>
        /// Sets a delegate to be used as a factory for creating new ITimeofDay instances.
        /// </summary>
        /// <param name="factory">The factory.</param>
        public static void SetFactory(Func<double, double, int, ITimeOfDay> factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Sets the default number of hours it takes to complete a full day.
        /// </summary>
        /// <param name="hours">The number of hours it takes to complete a full day.</param>
        public static void SetDefaultHoursPerDay(int hours)
        {
            _hoursPerDay = hours;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="TimePeriodManager"/> class.
        /// </summary>
        /// <param name="states">The states.</param>
        public TimePeriodManager(IEnumerable<ITimePeriod> states)
        {
            if (states == null)
            {
                throw new ArgumentNullException(nameof(states), "You must provide the TimeOfDayStateManager a collection of states that is not null.");
            }

            this.timeOfDayStates = states
                .OrderBy(item => item.StateStartTime.Hour)
                .ThenBy(item => item.StateStartTime.Minute)
                .ToList();
        }

        public ITimeOfDay CreateTimeOfDay(int hour, int minute, int hoursPerDay) => _factory(hour, minute, hoursPerDay);

        /// <summary>
        /// Looks at a supplied time of day and figures out what TimeOfDayState needs to be returned that matches the time of day.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <returns>
        /// Returns an instance of ITimePeriod that represents the current time of day in the game.
        /// </returns>
        public ITimePeriod GetTimeOfDayState(DateTime? currentTime = null)
        {
            ITimeOfDay time = TimePeriodManager._factory(currentTime.Value.Hour, currentTime.Value.Minute, _hoursPerDay);
            
            return this.GetTimePeriodForDay(time);
        }

        /// <summary>
        /// Looks at a supplied time of day and figures out what TimeOfDayState needs to be returned that matches the time of day.
        /// </summary>
        /// <param name="currentGameTime">The current time.</param>
        /// <returns>
        /// Returns an instance of ITimePeriod that represents the current time of day in the game.
        /// </returns>
        public ITimePeriod GetTimePeriodForDay(ITimeOfDay currentGameTime)
        {
            ITimePeriod inProgressState = null;
            ITimePeriod nextState = null;

            inProgressState = this.GetInProgressState(currentGameTime);
            nextState = this.GetNextState(currentGameTime);

            if (inProgressState != null)
            {
                return inProgressState;
            }
            else if (nextState != null && nextState.StateStartTime.Hour <= currentGameTime.Hour && nextState.StateStartTime.Minute <= currentGameTime.Minute)
            {
                return nextState;
            }

            return null;
        }

        /// <summary>
        /// Gets a state if there is one already in progress.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <returns>
        /// Returns an instance of ITimePeriod that represents the current time of day if an instance with a StartTime 
        /// before the current world-time can be found 
        /// </returns>
        ITimePeriod GetInProgressState(ITimeOfDay currentTime)
        {
            ITimePeriod inProgressState = null;
            foreach (ITimePeriod state in this.timeOfDayStates)
            {
                // If the state is already in progress, w
                if (state.StateStartTime.Hour <= currentTime.Hour ||
                    (state.StateStartTime.Hour <= currentTime.Hour &&
                    state.StateStartTime.Minute <= currentTime.Minute))
                {
                    if (inProgressState == null)
                    {
                        inProgressState = state;
                        continue;
                    }
                    else
                    {
                        if ((inProgressState.StateStartTime.Hour <= currentTime.Hour) ||
                            (inProgressState.StateStartTime.Hour == currentTime.Hour &&
                            inProgressState.StateStartTime.Minute <= currentTime.Minute))
                        {
                            inProgressState = state;
                        }
                    }
                }
            }

            return inProgressState;
        }

        /// <summary>
        /// Gets the state that is up next.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <returns>
        /// Returns an instance of ITimePeriod that represents the up coming time of day if an instance with a StartTime 
        /// after the current world-time can be found 
        /// </returns>
        ITimePeriod GetNextState(ITimeOfDay currentTime)
        {
            ITimePeriod nextState = null;
            foreach (ITimePeriod state in this.timeOfDayStates)
            {
                // If this state is a future state, then preserve it as a possible next state.
                if (state.StateStartTime.Hour > currentTime.Hour ||
                    (state.StateStartTime.Hour >= currentTime.Hour &&
                    state.StateStartTime.Minute > currentTime.Minute))
                {
                    // If we do not have a next state, set it.
                    if (nextState == null)
                    {
                        nextState = state;
                        continue;
                    }
                    else
                    {
                        // We have a next state, so we must check which is sooner.
                        if (nextState.StateStartTime.Hour > state.StateStartTime.Hour &&
                            nextState.StateStartTime.Minute >= state.StateStartTime.Minute)
                        {
                            nextState = state;
                        }
                    }
                }
            }

            return nextState;
        }
    }
}
