using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MudEngine.Core
{
    public interface IGame : IComponent, IDescriptor, IConfigurable, IAdaptable
    {
        GameState CurrentState { get; }
    }
}