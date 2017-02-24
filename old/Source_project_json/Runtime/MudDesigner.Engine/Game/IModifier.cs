namespace MudDesigner.Engine.Game
{
    /// <summary>
    /// Provides properties that define how a modifier is to be adjust a stat.
    /// </summary>
    public interface IModifier : IGameComponent
    {
        /// <summary>
        /// Gets the target that this modifier is intended for.
        /// </summary>
        IActor Target { get; }

        // TODO: Change to IUseable when the interface is created
        /// <summary>
        /// Gets the source that caused this modifier to be applied.
        /// </summary>
        IActor Source { get; }

        /// <summary>
        /// Gets the stat that this modifier affects on the Target.
        /// </summary>
        IStat AffectedStat { get; }

        /// <summary>
        /// Gets how much of the stat this modifier will change.
        /// </summary>
        double Amount { get; }

        /// <summary>
        /// Gets the duration in milliseconds.
        /// </summary>
        int Duration { get; }

        void AssignTarget(IActor target);

        void SetSource(IActor source);

        void SetAffectedStat(IStat affectedTargetStat);

        void SetAffectedAmount(double amount);

        void SetAffectDuration(int durationInMilliseconds);
    }
}
