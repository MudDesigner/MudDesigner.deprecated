namespace MudEngine.Routing
{
    public enum ResultType
    {
        // Indicates the route is done and the next potential match may run
        Done = 0,
        // Indicates this route accepts the route and does not want any additionally matched routes to run
        Accepted = 1,
        // Indicates this route failed and nothing should run after it.
        Failed = 2,
    }

    public interface IRouteResult
    {
        string[] FailureReason { get; }
        string AcceptReason { get; }
        ResultType ResultType { get; }
    }
}