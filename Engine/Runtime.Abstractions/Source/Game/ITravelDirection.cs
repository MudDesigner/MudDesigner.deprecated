namespace MudDesigner.Runtime.Game
{
    public interface ITravelDirection
    {
        string Direction { get; }

        ITravelDirection GetOppositeDirection();
    }
}
