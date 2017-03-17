namespace MudDesigner.Runtime.Game
{
    public struct MudDate : IDate
    {
        public MudDate(int day, int month, int year)
        {
            this.Day = day;
            this.Month = month;
            this.Year = year;
        }

        public int Month { get; }

        public int Day { get; }

        public int Year { get; }
    }
}
