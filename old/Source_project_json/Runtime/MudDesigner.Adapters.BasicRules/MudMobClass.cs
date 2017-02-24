using System;
using System.Collections.Generic;
using MudDesigner.Engine;
using MudDesigner.Engine.Game;

namespace MudDesigner.Adapters.BasicRules
{
    public class MudMobClass : MudComponent, IMobClass
    {
        protected List<IModifier> Modifiers { get; set; } = new List<IModifier>();

        public string Description { get; set; }

        public string Name { get; private set; }

        public void AddModifer(IModifier modifier)
        {
            if (modifier == null)
            {
                throw new ArgumentNullException(nameof(modifier), "Modifier argument can not be null.");
            }

            if (this.Modifiers == null)
            {
                this.Modifiers = new List<IModifier>();
            }

            if (this.Modifiers.Contains(modifier))
            {
                return;
            }

            this.Modifiers.Add(modifier);
        }

        public IModifier[] GetModifiers() => this.Modifiers?.ToArray();

        public void RemoveModifier(IModifier modifier)
        {
            if (modifier == null)
            {
                throw new ArgumentNullException(nameof(modifier), "Modifier argument can not be null.");
            }

            if (this.Modifiers == null)
            {
                this.Modifiers = new List<IModifier>();
            }

            this.Modifiers.Remove(modifier);
        }

        public void SetName(string name)
        {
            this.Name = name ?? string.Empty;
        }
    }
}
