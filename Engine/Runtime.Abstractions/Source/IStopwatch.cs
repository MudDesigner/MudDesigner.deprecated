namespace MudDesigner.Runtime
{
    public interface IStopwatch
    {
        void Start();

        void Stop();

        void Reset();

        int GetHours();

        int GetMinutes();

        int GetSeconds();

        int GetMilliseconds();
    }
}
