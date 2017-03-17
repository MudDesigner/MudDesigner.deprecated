using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.Runtime.Game
{
    internal class LocationWeather
    {
        internal List<IWeatherState> AvailableStates { get; } = new List<IWeatherState>();
        internal IWeatherState CurrentLocationWeather;
    }

    public class WeatherAdapter : IAdapter
    {
        private Dictionary<ILocation, LocationWeather> locationWeather;
        private ITimeOfDay lastWeatherUpdate;

        public WeatherAdapter(IMessageBrokerFactory brokerFactory)
        {
            // TODO: Add WeatherAdapterConfiguration to support configuration
            // Things like default update frequency.
            this.MessageBroker = brokerFactory.CreateBroker();
            this.locationWeather = new Dictionary<ILocation, LocationWeather>();
        }

        public string Name => "Mud Weather Adapter";

        public string Description => "Provides Weather functionality to the world.";

        public IMessageBroker MessageBroker { get; }

        // TODO: Add params WeatherLocation[] that allows for setting the update frequency per-location.
        public void AddWeatherToLocation(ILocation location, params IWeatherState[] weather)
        {
            if (!this.locationWeather.TryGetValue(location, out var weatherLocation))
            {
                weatherLocation = new LocationWeather();
                this.locationWeather.Add(location, weatherLocation);
            }

            weatherLocation.AvailableStates.AddRange(weather);
        }

        public Task Configure()
        {
            return Task.CompletedTask;
        }

        public Task Delete()
        {
            return Task.CompletedTask;
        }

        public Task Initialize()
        {
            foreach(KeyValuePair<ILocation, LocationWeather> currentLocationWeather in this.locationWeather)
            {
                IWeatherState startingState = currentLocationWeather.Value.AvailableStates.AnyOrDefaultFromWeight(state => state.OccurrenceProbability);
                currentLocationWeather.Value.CurrentLocationWeather = startingState;
            }

            return Task.CompletedTask;
        }

        public Task Update(IGame game)
        {
            IUniverseClock currentClock = game.UniverseClock;
            ulong universeAge = currentClock.GetUniverseAgeAsMilliseconds();
            
            // TODO: Determine if enough time has passed since we last checked the weather

            // Update the weather if applicable
            foreach(KeyValuePair<ILocation, LocationWeather> currentLocationWeather in this.locationWeather)
            {
                // if ( currentLocationWeather.LastUpdatedTime) // Is this locations update frequency say we need to update?
                IWeatherState newState = currentLocationWeather.Value.AvailableStates.AnyOrDefaultFromWeight(state => state.OccurrenceProbability);
                if (currentLocationWeather.Value.CurrentLocationWeather == newState)
                {
                    // Skip: Weather state didn't change, so we don't want to broadcast weather state change notifications by replacing
                    // it with the same state.
                    continue;
                }

                currentLocationWeather.Value.CurrentLocationWeather = newState;
            }

            return Task.CompletedTask;
        }
    }
}
