using System;
using System.Collections.Generic;
using System.Text;

namespace MudDesigner.Runtime.Game
{
    public class CurrentUniverseTimeMessage : IMessage
    {
        public CurrentUniverseTimeMessage(ITimeOfDay universeTime) => this.CurrentTime = universeTime;

        public ITimeOfDay CurrentTime { get; private set; }

        public void SetCurrentTime(ITimeOfDay timeOfDay) => this.CurrentTime = timeOfDay;

        public object GetContent() => this.CurrentTime;
    }
}
