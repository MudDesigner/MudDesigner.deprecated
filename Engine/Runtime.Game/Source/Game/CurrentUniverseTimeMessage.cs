namespace MudDesigner.Runtime.Game
{
    public struct CurrentUniverseTimeMessage : IMessage
    {
        public CurrentUniverseTimeMessage(ITimeOfDay universeTime) => this.CurrentTime = universeTime;

        public ITimeOfDay CurrentTime { get; private set; }

        public void SetCurrentTime(ITimeOfDay timeOfDay) => this.CurrentTime = timeOfDay;

        public object GetContent() => this.CurrentTime;
    }
}
