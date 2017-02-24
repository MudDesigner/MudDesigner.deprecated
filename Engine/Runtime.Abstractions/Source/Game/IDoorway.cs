using System.Threading.Tasks;

namespace MudDesigner.Runtime.Game
{
    public interface IDoorway
    {
        ITravelDirection DepartureDirection { get; }

        IRoom DepartureRoom { get; }

        IRoom ArrivalRoom { get; }

        Task ConnectRooms(ITravelDirection departureDirection, IRoom departureRoom, IRoom arrivalRoom);

        Task DisconnectRooms();
    }
}
