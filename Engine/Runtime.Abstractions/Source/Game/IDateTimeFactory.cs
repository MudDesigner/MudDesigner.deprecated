namespace MudDesigner.Runtime.Game
{
    public interface IDateTimeFactory
    {
        ITimeOfDay CreateTimeOfDay(int hour);

        ITimeOfDay CreateTimeOfDay(int hour, int minute);

        ITimeOfDay CreateTimeOfDay(int hour, int minute, int second);

        ITimeOfDay CreateTimeOfDay(int hour, int minute, int second, int millisecond);

        ITimeOfDay CreateTimeOfDay(IUniverseClock universeClock, ITimeOfDay timezoneOffset);

        IDate CreateDate(int day, int month, int year);

        IDateTime CreateDateTime(ITimeOfDay timeOfDay, IDate date);

        IDateTime CreateDateTime(ulong elapsedMilliseconds, int hoursPerDay);
    }
}
