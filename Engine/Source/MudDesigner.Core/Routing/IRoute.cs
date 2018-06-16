using System.Threading.Tasks;

namespace MudEngine.Routing
{
    public interface IRoute : IDescriptor
    {
        Task<IRouteResult> Handle(IRouteContext context);
    }
}