using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MudDesigner.Runtime.Game
{
    public interface IRoom
    {
        ILocation Owner { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is sealed. A sealed room will throw an exception if anything tries to add or remove an actor.
        /// A sealed room prevent actors from entering or leaving it.
        /// </summary>
        bool IsSealed { get; }

        Task SealRoom();

        Task UnsealRoom();

        IEnumerable<IDoorway> GetDoorways();

        Task AddDoorway(IDoorway doorway);

        Task RemoveDoorway(IDoorway doorway);

        Task RemoveDoorway(ITravelDirection travelDirection);
    }
}