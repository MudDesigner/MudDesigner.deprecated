namespace MudDesigner.Runtime.Game
{
    public struct MudDateTime : IDateTime
    {
        public MudDateTime(ITimeOfDay timeOfDay, IDate date)
        {
            this.Time = timeOfDay;
            this.Date = date;
        }

        public IDate Date { get; }

        public ITimeOfDay Time { get; }
    }
}
