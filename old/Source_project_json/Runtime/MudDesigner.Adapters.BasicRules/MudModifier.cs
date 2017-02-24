using System;
using System.Threading.Tasks;
using MudDesigner.Engine.Game;

namespace MudDesigner.Adapters.BasicRules
{
    public class MudModifier : GameComponent, IModifier
    {
        public IStat AffectedStat { get; private set; }

        public int Duration { get; private set; }

        public IActor Source { get; private set; }

        public IActor Target { get; private set; }

        public double Amount { get; private set; }

        public void AssignTarget(IActor target)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target), "The actor provided was null and can not be used.");
            }

            this.Target = target;
        }

        public void SetAffectDuration(int durationInMilliseconds)
        {
            if (durationInMilliseconds < 0.0000M)
            {
                throw new ArgumentOutOfRangeException(nameof(durationInMilliseconds), "You can not have a negative duration");
            }

            this.Duration = durationInMilliseconds;
        }

        public void SetAffectedAmount(double amount)
        {
            this.Amount = amount;
        }

        public void SetAffectedStat(IStat affectedTargetStat)
        {
            if (affectedTargetStat == null)
            {
                throw new ArgumentNullException(nameof(affectedTargetStat), "You can not use a null Stat for this modifier");
            }

            if (string.IsNullOrEmpty(affectedTargetStat.Name))
            {
                throw new ArgumentOutOfRangeException(nameof(affectedTargetStat), "The provided Stat does not have a name assigned to it.");
            }

            this.AffectedStat = affectedTargetStat;
        }

        public void SetSource(IActor source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source), "The source for the modifier can not be null");
            }

            // TODO: Create internal validators that can be used to validate the interface properties and methods.
            if (string.IsNullOrEmpty(source.Name))
            {
                throw new ArgumentOutOfRangeException(nameof(source), "The source must have a valid name in order to be assigned as a modifier source.");
            }

            this.Source = source;
        }

        protected override Task Load()
        {
            return Task.FromResult(0);
        }

        protected override Task Unload()
        {
            return Task.FromResult(0);
        }
    }
}
