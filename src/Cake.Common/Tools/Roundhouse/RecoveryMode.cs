namespace Cake.Common.Tools.Roundhouse
{
    /// <summary>
    /// Defines the recovery model for SQL Server
    /// </summary>
    public enum RecoveryMode
    {
        /// <summary>
        /// Doesn't change the mode
        /// </summary>
        NoChange,

        /// <summary>
        /// Does not create backup before migration
        /// </summary>
        Simple,

        /// <summary>
        /// Creates log backup before migration
        /// </summary>
        Full
    }
}