
namespace MudDesigner.Engine.Game
{
    public interface IRace : IDescriptor, IComponent
    {
        IColor HairColor { get; set; }

        IColor SkinColor { get; set; }

        IColor EyeColor { get; set; }

        void AddModifer(IModifier modifier);

        void RemoveModifier(IModifier modifier);

        IModifier[] GetModifiers();
    }
}
