namespace MudDesigner.Engine.Game
{
    public interface ISecurity
    {
        bool ActorHasAccessControl(IActor target, IAccessControl accessControl);
        
        void GrantAccessControl(IActor target, IAccessControl accessControl);
        
        void RevokeAccessControl(IActor target, IAccessControl accessControl);
    }
}