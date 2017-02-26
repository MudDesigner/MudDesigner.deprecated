using System;
using System.Collections.Generic;
using System.Text;

namespace MudDesigner.Runtime.Game
{
    public interface IPlayerFactory
    {
        IPlayer CreatePlayer();
    }
}
