using System.Collections.Generic;

namespace MudDesigner.Engine.Game
{
    public interface IActor : IGameComponent
    {
        IRoom CurrentRoom { get; }
        
        void MoveToRoom(IRoom room);
        
        List<IGameComponent> Components { get; }
    }
}
