namespace MudEngine.Transport
{
    public class ConsolePipeline : ITransportPipeline
    {
        public ITransportReader Input { get; }

        public ITransportWriter Output { get; }
    }
}