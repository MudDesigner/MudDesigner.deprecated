namespace MudDesigner.Engine.Game
{
    public interface IModifierFactory
    {
        IModifier CreateModifier(IActor target, IActor source, int amount);

        IModifier CreateModifier(IActor target, IActor source, int amount, int duration);

        IModifier CreateModifier(IActor target, IActor source, IStat stat, int amount);

        IModifier CreateModifier(IActor target, IActor source, IStat stat, int amount, int duration);
    }
}