
namespace MudDesigner.Engine.Game
{
    public interface IMountPoint : IDescriptor, IComponent
    {

        bool AllowsMultiple { get; set; }

        int MaxMountings { get; set; }

        IAttachment[] GetMountedItems();

        void Mount(IAttachment attachment);
    }
}
