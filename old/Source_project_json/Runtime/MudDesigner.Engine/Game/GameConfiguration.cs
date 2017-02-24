//-----------------------------------------------------------------------
// <copyright file="GameInformation.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine.Game
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides meta-information for the currently running game.
    /// </summary>
    public class GameConfiguration : IGameConfiguration
    {
        List<IAdapter> components;

        public GameConfiguration()
        {
            this.Version = new Version("1.0.0.0");
            this.components = new List<IAdapter>();
        }

        /// <summary>
        /// Gets or sets the name of the game being played.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the game.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the current version of the game.
        /// </summary>
        public Version Version { get; set; }

        /// <summary>
        /// Gets or sets the website that users can visit to get information on the game.
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// Gets the game adapter components that have been registered.
        /// </summary>
        /// <returns>Returns an array of adapter components</returns>
        public IAdapter[] GetAdapters() => this.components.ToArray();

        /// <summary>
        /// Tells the game configuration that a specific adapter component must be used by the game.
        /// A new instance of TConfigComponent will be created when the game starts.
        /// </summary>
        /// <typeparam name="TAdapter">The type of the adapter component to use.</typeparam>
        public void UseAdapter<TAdapter>() where TAdapter : class, IAdapter, new() => this.components.Add(new TAdapter());

        /// <summary>
        /// Tells the game configuration that a specific adapter component must be used by the game.
        /// </summary>
        /// <typeparam name="TAdapter">The type of the adapter component.</typeparam>
        /// <param name="component">The component instance you want to use.</param>
        public void UseAdapter<TAdapter>(TAdapter component) where TAdapter : class, IAdapter
        {
            if (component == null)
            {
                throw new ArgumentNullException(nameof(component), $"The adapter component provided of Type {component.GetType().Name} was null.");
            }

            this.components.Add(component);
        }

        public void UseAdapters(IEnumerable<IAdapter> adapters)
        {
            foreach(IAdapter adapter in adapters)
            {
                this.UseAdapter(adapter);
            }
        }
        
        public Task Configure(IGame game)
        {
            return Task.FromResult(0);
        }
    }
}
