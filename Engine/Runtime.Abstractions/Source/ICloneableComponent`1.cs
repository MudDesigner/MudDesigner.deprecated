namespace MudDesigner.Runtime
{
    public interface ICloneableComponent<TClone>
    {
        TClone Clone();
    }
}
