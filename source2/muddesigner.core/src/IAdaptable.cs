using System.Collections.Generic;

namespace MudEngine
{
    public interface IAdaptable
    {
        IAdapter[] GetAdapters();

        void UseAdapters(params IAdapter[] adaptersToUse);
    }
}