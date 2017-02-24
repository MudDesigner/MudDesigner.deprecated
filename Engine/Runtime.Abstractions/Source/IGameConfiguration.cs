namespace MudDesigner.Runtime
{
    public interface IGameConfiguration
    {
        string Name { get; set; }

        string Description { get; set; }

        string Version { get; set; }

        string WebSite { get; set; }
    }
}
