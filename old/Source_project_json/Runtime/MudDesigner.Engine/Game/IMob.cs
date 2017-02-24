
namespace MudDesigner.Engine.Game
{
    public interface IMob : IActor
    {
        IMobClass MobClass { get; }
        
        IRace Race { get; }

        IGender Gender { get; }

        void SetGender(IGender gender);

        void SetRace(IRace race);

        void AddMountPoint(IMountPoint mountPoint);

        IMountPoint[] GetMountPoints();

        IMountPoint FindMountPoint(string pointName);

        void AddAbility(IStat ability);

        IStat[] GetAbilities();
    }
}
