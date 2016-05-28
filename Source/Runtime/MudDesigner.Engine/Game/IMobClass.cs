using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.Engine.Game
{
    public interface IMobClass : IDescriptor, IComponent
    {
        void AddModifer(IModifier modifier);

        void RemoveModifier(IModifier modifier);

        IModifier[] GetModifiers();
    }
}
