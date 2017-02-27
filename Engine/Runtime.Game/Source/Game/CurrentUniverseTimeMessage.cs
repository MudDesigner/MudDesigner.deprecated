using System;
using System.Collections.Generic;
using System.Text;

namespace MudDesigner.Runtime.Game
{
    public class CurrentUniverseTimeMessage : IMessage
    {
        public CurrentUniverseTimeMessage(ITimeOfDay universeTime) => this.CurrentTime = universeTime;

        public ITimeOfDay CurrentTime { get; }

        public object GetContent() => this.CurrentTime;
    }
}
