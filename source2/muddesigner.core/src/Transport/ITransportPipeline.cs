namespace MudEngine.Transport
{
    public interface ITransportPipeline
    {
        ITransportReader Input { get; }
        ITransportWriter Output { get; }
    }
}