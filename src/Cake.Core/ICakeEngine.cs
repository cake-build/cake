using System.Collections.Generic;

namespace Cake.Core
{
    /// <summary>
    /// Represents the Cake engine.
    /// </summary>
    public interface ICakeEngine : ICakeContext
    {
        /// <summary>
        /// Gets all registered tasks.
        /// </summary>
        /// <value>The registered tasks.</value>
        IReadOnlyList<CakeTask> Tasks { get; }

        /// <summary>
        /// Registers a new task.
        /// </summary>
        /// <param name="name">The name of the task.</param>
        /// <returns>A <see cref="CakeTaskBuilder{ActionTask}"/>.</returns>
        CakeTaskBuilder<ActionTask> Task(string name);

        /// <summary>
        /// Runs the specified target.
        /// </summary>
        /// <param name="target">The target to run.</param>
        /// <returns>The resulting report.</returns>
        CakeReport RunTarget(string target);
    }
}
