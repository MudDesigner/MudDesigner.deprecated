namespace MudDesigner.Engine.Game
{
    public interface IStat : IDescriptor, IInitializableComponent, IComponent
    {
        string Abbreviation { get; set; }

        int BaseScore { get; }

        int BaseModifier { get; }

        int StatScore { get; }

        int ModifiedScore { get; }

        void AddModifier(IModifier modifier);

        IModifier[] GetModifiers();

        void SetScore(int score);
    }
}
