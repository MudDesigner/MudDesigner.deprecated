using System;
using System.Threading.Tasks;

namespace MudEngine.Routing
{
    public interface IRouter : IAdapter
    {
        // Handlers are ephemeral and will not be re-used between requests.
        void RegisterHandler<THandler>() where THandler : IRoute;

        Task<IRouteContext> HandleRoutes(params IRoute[] routes);
    }
}