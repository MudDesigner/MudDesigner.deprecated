namespace MudDesigner.Engine.Game
{    
    public class AutosaveMessage<TSavedComponent> : IMessage<TSavedComponent> where TSavedComponent : class
    {
        public AutosaveMessage(TSavedComponent component)
        {
            this.Content = component;
        }
        
        /// <summary>
        /// The component that is being auto-saved.
        /// </summary>
        public TSavedComponent Content { get; private set; }

        public object GetContent()
        {
            return this.Content;
        }
    }
}