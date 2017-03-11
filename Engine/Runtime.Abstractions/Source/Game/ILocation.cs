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
