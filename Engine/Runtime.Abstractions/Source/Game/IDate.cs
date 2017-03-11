using System;
using System.Collections.Generic;
using System.Text;

namespace MudDesigner.Runtime.Game
{
    public interface IDate
    {
        int Month { get; }

        int Day { get; }

        int Year { get; }
    }
}
