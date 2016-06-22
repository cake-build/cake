namespace Cake.Core.Diagnostics
{
    /// <summary>
    ///     Represents a pipeline the logs to registered <see cref="ICakeLog" /> instances.
    /// </summary>
    /// <remarks>
    ///     The ICakeLogPipeline interface represents a pipeline of <see cref="ICakeLog" /> instances that the system will log
    ///     to individually.  This allows the user to hook their own <see cref="ICakeLog" /> implementation into the system
    ///     either temporarily or over the entire build.
    /// </remarks>
    public interface ICakeLogPipeline
    {
        /// <summary>
        ///     Gets the <see cref="ICakeLog" /> implementation that represents the pipeline.
        /// </summary>
        ICakeLog CakeLog { get; }

        /// <summary>
        ///     Adds a <see cref="ICakeLog" /> instance to the pipeline.
        /// </summary>
        /// <param name="toAdd">The pipeline instance to add.</param>
        void AddLog(ICakeLog toAdd);

        /// <summary>
        ///     Removes a <see cref="ICakeLog" /> instance from the pipeline.
        /// </summary>
        /// <param name="toRemove">The pipeline instance to remove.</param>
        void RemoveLog(ICakeLog toRemove);
    }
}