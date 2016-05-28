//-----------------------------------------------------------------------
// <copyright file="WorldManager.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MudDesigner.Engine;
using MudDesigner.Engine.Game;

namespace MudDesigner.Adapters.BasicRules
{
    /// <summary>
    /// Provides methods for creating and maintaining the worlds in the game.
    /// </summary>
    public sealed class WorldManager : AdapterBase
    {
        /// <summary>
        /// A collection of worlds added to the manager
        /// </summary>
        List<IWorld> worlds;

        /// <summary>
        /// The world factory used to create new instances of IWorld
        /// </summary>
        IWorldFactory worldFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldManager"/> class.
        /// </summary>
        /// <param name="worldFactory">The factory responsible for creating new worlds.</param>
        public WorldManager(IWorldFactory worldFactory)
        {
            this.worlds = new List<IWorld>();
            this.worldFactory = worldFactory;
        }

        /// <summary>
        /// Gets a collection of worlds associated with this game.
        /// </summary>
        public IWorld[] Worlds { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public override string Name => "World Manager";

        public override void Configure()
        {
            // Stubbed.
        }

        /// <summary>
        /// Creates an initialized world ready for use.
        /// </summary>
		/// <param name="worldName">The name of the world.</param>
		/// <param name="gameDayToRealWorldHourRatio">The game day to real world hours ratio.</param>
        /// <param name="hoursPerDay">The number of hours per day.</param>
        /// <returns>Returns an IWorld instance</returns>
        /// <exception cref="System.InvalidOperationException">$The {this.Name} was not provided an IWorldFactory to use.</exception>
        public Task<IWorld> CreateWorld(string worldName, double gameDayToRealWorldHourRatio, int hoursPerDay)
        {
            if (this.worldFactory == null)
            {
                throw new InvalidOperationException($"The {this.Name} was not provided an IWorldFactory to use.");
            }

            return this.worldFactory.CreateWorld(
                worldName,
                gameDayToRealWorldHourRatio,
                hoursPerDay);
        }

        /// <summary>
        /// Adds a given world to the games available worlds.
        /// </summary>
        /// <param name="world">The world to give the game.</param>
        /// <returns>Returns an awaitable Task</returns>
        public Task AddWorld(IWorld world)
        {
            if (this.worlds.Contains(world))
            {
                return Task.FromResult(0);
            }


            if (System.Math.Abs(world.GameDayToRealHourRatio - default(double)) <= 0.000)
            {
                var exception = new InvalidOperationException("You assign the ratio between an in-game day to a real-world hour.");
                exception.Data.Add(this, world);
                throw exception;
            }

            if (world.HoursPerDay == 0)
            {
                var exception = new InvalidOperationException("You must define how many hours it takes to make up a single day in the world.");
                exception.Data.Add(this, world);
                throw exception;
            }
            
            this.worlds.Add(world);
            return Task.FromResult(0);
        }

        /// <summary>
        /// Adds a collection of worlds to the game.
        /// </summary>
        /// <para>
        /// If a world already exists in the game, it is ignored.
        /// </para>
        /// <param name="worlds">The worlds collection to add.</param>
        /// <returns>Returns an awaitable Task</returns>
        public async Task AddWorlds(IEnumerable<IWorld> worlds)
        {
            foreach(IWorld world in worlds)
            {
                await this.AddWorld(world);
            }
        }

        /// <summary>
        /// Initializes the component.
        /// </summary>
        /// <returns>Returns an awaitable Task</returns>
        public override Task Initialize()
        {
            if (this.worlds == null)
            {
                this.worlds = new List<IWorld>();
            }

            return Task.FromResult(0);
        }

        /// <summary>
        /// Starts this adapter and allows it to run.
        /// All of the worlds will be initialized and will start running.
        /// </summary>
        /// <param name="game">The an instance of an initialized game.</param>
        /// <returns>
        /// Returns an awaitable Task
        /// </returns>
        public override async Task Start(IGame game)
        {
            foreach(IWorld world in this.worlds)
            {
                await world.Initialize();
            }
        }

        /// <summary>
        /// Lets this instance know that it is about to go out of scope and disposed.
        /// The instance will perform clean-up of its resources in preparation for deletion.
        /// </summary>
        /// <para>
        /// Informs the component that it is no longer needed, allowing it to perform clean up.
        /// Objects registered to one of the two delete events will be notified of the delete request.
        /// </para>
        /// <returns>Returns an awaitable Task</returns>
        public override async Task Delete()
        {
            foreach (IWorld world in this.worlds)
            {
                await world.Delete();
            }
        }
    }
}
