//-----------------------------------------------------------------------
// <copyright file="RainyWeather.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MudDesigner.Engine.Game
{
    /// <summary>
    /// Represents rainy weather.
    /// </summary>
    public struct RainyWeather : IWeatherState
    {
        /// <summary>
        /// Gets the occurrence probability of this weather state occurring in an environment.
        /// The higher the probability relative to other weather states, the more likely it is going to occur.
        /// </summary>
        public double OccurrenceProbability => 30;

        /// <summary>
        /// Gets the name of the weather state.
        /// </summary>
        public string Name => "Rainy";
    }
}
