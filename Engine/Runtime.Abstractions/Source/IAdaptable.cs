using System.Collections.Generic;

namespace MudDesigner.Runtime
{
    public interface IAdaptable
    {
        IEnumerable<IAdapter> GetAdapters();

        void UseAdapter<TAdapter>(TAdapter adapter) where TAdapter : class, IAdapter;

        void UseAdapters(params IAdapter[] adaptersToUse);
    }
}
