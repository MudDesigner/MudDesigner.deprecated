namespace MudEngine.Routing
{
    public interface IRouteContext
    {
        string Route { get; }

        string[] Components { get; }

        IRouter Router { get; }

        IRouteResult Results { get; }

        IRouteResult FailRoute(string reason);

        IRouteResult AcceptRoute();

        IRouteResult AcceptRoute(string message);

        IRouteResult Done();
    }
}