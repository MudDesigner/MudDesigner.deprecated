using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MudDesigner.Engine.Game;

namespace MudDesigner.Adapters.BasicRules
{
    public class MudRace : GameComponent, IRace
    {
        List<IModifier> modifiers = new List<IModifier>();

        public IColor EyeColor { get; set; }

        public IColor HairColor { get; set; }

        public IColor SkinColor { get; set; }

        public void AddModifer(IModifier modifier)
        {
            if (modifier == null)
            {
                throw new ArgumentNullException(nameof(modifier), "You can not provide a null modifier to this race.");
            }

            if (this.modifiers.Contains(modifier))
            {
                return;
            }

            this.modifiers.Add(modifier);
        }

        public IModifier[] GetModifiers() => this.modifiers.ToArray();

        public void RemoveModifier(IModifier modifier)
        {
            if (modifier == null)
            {
                return;
            }

            this.modifiers.Remove(modifier);
        }

        protected override Task Load()
        {
            return Task.FromResult(0);
        }

        protected override Task Unload()
        {
            return Task.FromResult(0);
        }
    }
}
