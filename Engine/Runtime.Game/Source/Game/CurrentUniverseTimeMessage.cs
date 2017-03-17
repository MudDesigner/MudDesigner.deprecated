namespace MudDesigner.Runtime.Game
{
    public struct CurrentUniverseTimeMessage : IMessage
    {
        public CurrentUniverseTimeMessage(IDateTime universeTime) => this.CurrentTime = universeTime;

        public IDateTime CurrentTime { get; private set; }

        public object GetContent() => this.CurrentTime;
    }
}
