using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.Engine.Game
{
    public interface IColor : IDescriptor
    {
        string Color { get; }

        void SetFromString(string color);

        void SetFromColor(IColor color);
    }
}
