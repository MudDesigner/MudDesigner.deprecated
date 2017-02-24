//-----------------------------------------------------------------------
// <copyright file="IWeatherState.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine.Game
{
    // TODO: All IWeatherState implementations should be converted from classes to structs
    /// <summary>
    /// Exposes properties that represent a state that the weather may be in.
    /// </summary>
    public interface IWeatherState
    {
        /// <summary>
        /// Gets the occurrence probability of this weather state occurring in an environment.
        /// The higher the probability relative to other weather states, the more likely it is going to occur.
        /// </summary>
        double OccurrenceProbability { get; }

        /// <summary>
        /// Gets the name of the weather state.
        /// </summary>
        string Name { get; }
    }
}