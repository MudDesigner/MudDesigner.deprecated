using System;
using System.Collections.Generic;
using System.Text;

namespace MudDesigner.Runtime.Game
{
    public interface IPlayer
    {
        IRoom CurrentRoom { get; }
    }
}
