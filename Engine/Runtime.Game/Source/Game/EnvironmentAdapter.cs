using System.Collections.Generic;
using System.Threading.Tasks;

namespace MudDesigner.Runtime.Game
{
    public class EnvironmentAdapter : IAdapter
    {
        private IEnumerable<IWeatherState> weatherStates;
        private IGame game;

        private List<ILocation> locations;

        public EnvironmentAdapter(IMessageBrokerFactory brokerFactory, IEnumerable<IWeatherState> weatherStates)
        {
            this.MessageBroker = brokerFactory.CreateBroker();
            this.weatherStates = weatherStates;
        }

        public string Name => "Mud Weather";

        public string Description => "Weather system for Mud Designer";

        public IMessageBroker MessageBroker { get; }

        public Task Configure()
        {
            return Task.CompletedTask;
        }

        public Task Delete()
        {
            return Task.CompletedTask;
        }

        public async Task Initialize()
        {
            foreach(ILocation location in this.locations)
            {
                await location.Initialize();
            }
        }

        public async Task Update(IGame game)
        {
            foreach (ILocation location in this.locations)
            {
                // todo: figure out how to update the calendar in a better way

                await location.Update(game);
            }
        }

        public void AddLocation(ILocation location)
        {
            this.locations.Add(location);
        }
    }
}
