using System;
using MudDesigner.Engine.Game;

namespace MudDesigner.Adapters.BasicRules
{
    public class MudColor : IColor
    {
        public string Color { get; private set; }

        public string Description { get; set; }

        public string Name { get; private set; }

        public void SetFromColor(IColor color)
        {
            if (color == null)
            {
                throw new ArgumentNullException(nameof(color), "The color parameter can not be null.");
            }

            this.Color = color.Color;
        }

        public void SetFromString(string color)
        {
            this.Color = color ?? string.Empty;
        }

        public void SetName(string name)
        {
            this.Name = name ?? string.Empty;
        }
    }
}
