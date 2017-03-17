namespace MudDesigner.Runtime
{
    public interface IStopwatch
    {
        void Start();

        void Stop();

        void Reset();

        ulong GetHours();

        ulong GetMinutes();

        ulong GetSeconds();

        ulong GetMilliseconds();
    }
}
