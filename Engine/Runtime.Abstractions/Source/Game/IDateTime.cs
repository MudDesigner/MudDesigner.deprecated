using System;
using System.Collections.Generic;
using System.Text;

namespace MudDesigner.Runtime.Game
{
    public interface IDateTime
    {
        IDate Date { get; }

        ITimeOfDay Time { get; }
    }
}
