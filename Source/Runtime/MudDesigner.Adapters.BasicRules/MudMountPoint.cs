using System;
using System.Collections.Generic;
using MudDesigner.Engine.Game;

namespace MudDesigner.Adapters.BasicRules
{
    public class MudMountPoint : IMountPoint
    {
        private List<IAttachment> mountedAttachments = new List<IAttachment>();

        public MudMountPoint()
        {
            this.CreationDate = DateTime.Now;
            this.Id = Guid.NewGuid();
        }

        public bool AllowsMultiple { get; set; }

        public DateTime CreationDate { get; }

        public string Description { get; set; }

        public Guid Id { get; }

        public bool IsEnabled { get; private set; }

        public int MaxMountings { get; set; }

        public string Name { get; private set; }

        public double TimeAlive => DateTime.Now.Subtract(this.CreationDate).TotalSeconds;

        public void Disable()
        {
            this.IsEnabled = false;
        }

        public void Enable()
        {
            this.IsEnabled = true;
        }

        public IAttachment[] GetMountedItems() => this.mountedAttachments.ToArray();

        public void Mount(IAttachment attachment)
        {
            if (attachment == null)
            {
                throw new ArgumentNullException(nameof(attachment), "A null attachment can not be null");
            }

            if (this.mountedAttachments.Contains(attachment))
            {
                return;
            }

            this.mountedAttachments.Add(attachment);
        }

        public void SetName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name), "You can not assign a mount point an empty Name");
            }

            this.Name = name;
        }
    }
}
