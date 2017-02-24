using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.Runtime.Game
{
    public interface ILocation : IDescriptor, IInitializable, IComponent, IPersistable
    {
        ILocation Owner { get; }

        ICalendar Calendar { get; set; }

        IWeatherState CurrentWeather { get; }

        /// <summary>
        /// How often in minutes within the game world the game will update the weather state.
        /// </summary>
        int WeatherUpdateFrequency { get; }

        IEnumerable<IWeatherState> GetWeatherStates();

        Task AddWeatherState(IWeatherState weatherState);

        Task RemoveWeatherState(IWeatherState weatherState);

        IEnumerable<ILocation> GetChildrenLocations();

        Task AddLocation(ILocation location);

        Task RemoveLocation(ILocation location);

        bool HasChildLocation(ILocation location);

        IEnumerable<IRoom> GetRooms();

        bool HasRoom(IRoom room);

        Task AddRoom(IRoom room);

        Task RemoveRoom(IRoom room);
    }
}
