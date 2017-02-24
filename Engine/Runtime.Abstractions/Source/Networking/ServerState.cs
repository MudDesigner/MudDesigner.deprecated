namespace MudDesigner.Runtime.Networking
{
    public enum ServerState
    {
        None = 0,
        Configuring = 1,
        Configured = 2,
        Starting = 3,
        Running = 4,
        Stopping = 5,
        Stopped = 6
    }
}
