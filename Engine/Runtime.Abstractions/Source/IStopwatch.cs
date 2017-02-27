namespace MudDesigner.Runtime
{
    public interface IStopwatch
    {
        void Start();

        void Stop();

        void Reset();

        long GetHours();

        long GetMinutes();

        long GetSeconds();

        long GetMilliseconds();
    }
}
