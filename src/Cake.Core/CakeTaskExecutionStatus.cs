namespace Cake.Core
{
    /// <summary>
    /// The execution status of a <see cref="CakeTask"/>.
    /// </summary>
    public enum CakeTaskExecutionStatus
    {
        /// <summary>
        /// The task was executed.
        /// </summary>
        Executed,

        /// <summary>
        /// The task delegated execution.
        /// </summary>
        Delegated,

        /// <summary>
        /// The task was skipped.
        /// </summary>
        Skipped
    }
}