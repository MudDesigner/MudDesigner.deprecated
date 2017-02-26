using System;
using MudDesigner.Runtime;

namespace Tests.Fakes
{
    internal class MessageFake : IMessage<string>
    {
        public string Content { get; set; }

        public object GetContent() => this.Content;
    }
}
