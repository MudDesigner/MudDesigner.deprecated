using MudDesigner.Engine.Game;

namespace MudDesigner.Adapters.BasicRules
{
    public class ModifierFactory : IModifierFactory
    {
        public IModifier CreateModifier(IActor target, IActor source, int amount)
        {
            var modifier = new MudModifier();
            modifier.AssignTarget(target);
            modifier.SetSource(source);
            modifier.SetAffectedAmount(amount);

            return modifier;
        }

        public IModifier CreateModifier(IActor target, IActor source, int amount, int duration)
        {
            IModifier modifier = this.CreateModifier(target, source, amount);
            modifier.SetAffectDuration(duration);

            return modifier;
        }

        public IModifier CreateModifier(IActor target, IActor source, IStat stat, int amount)
        {
            IModifier modifier = this.CreateModifier(target, source, amount);
            modifier.SetAffectedStat(stat);

            return modifier;
        }

        public IModifier CreateModifier(IActor target, IActor source, IStat stat, int amount, int duration)
        {
            IModifier modifier = this.CreateModifier(target, source, stat, amount);
            modifier.SetAffectDuration(duration);

            return modifier;
        }
    }
}