namespace MudEngine.Transport
{
    public class ConsolePipeline : ITransportPipeline
    {
        public ITransportReader Input { get; } = new ConsoleReader();

        public ITransportWriter Output { get; } = new ConsoleWriter();
    }
}