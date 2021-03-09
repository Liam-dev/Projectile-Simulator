namespace Simulator.UserInterface
{
    /// <summary>
    /// Structure which in which Editor preferences are stored.
    /// </summary>
    public struct EditorPreferences
    {
        /// <summary>
        /// Gets or sets automatic naming of objects property.
        /// </summary>
        public bool AutoName { get; set; }

        /// <summary>
        /// Gets or sets whether projectile trajectories should be shown.
        /// </summary>
        public bool ShowTrajectories { get; set; }
    }
}