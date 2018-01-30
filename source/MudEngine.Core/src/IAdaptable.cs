using System.Collections.Generic;

namespace MudEngine.Core
{
    public interface IAdaptable
    {
        IAdapter[] GetAdapters();
        void UseAdapters(params IAdapter[] adaptersToUse);
    }
}