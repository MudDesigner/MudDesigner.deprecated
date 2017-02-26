using MudDesigner.Runtime.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace Runtime.Adapter.Telnet.Tests.Fakes
{
    public class FakePlayer : IPlayer
    {
        public IRoom CurrentRoom => throw new NotImplementedException();
    }
}
