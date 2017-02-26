using System;
using MudDesigner.Runtime;

namespace Tests.Fakes
{
    internal class ResettableMessageFake : IMessage<string>, IReusable
    {
        public string Content { get; set; }

        public object GetContent() => this.Content;

        public void PrepareForReuse()
        {
            this.Content = "Reused";
        }
    }
}
