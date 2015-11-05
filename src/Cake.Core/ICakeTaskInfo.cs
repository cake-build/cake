using System.Collections.Generic;

namespace Cake.Core
{
    /// <summary>
    /// Provides descriptive properties about a cake task
    /// </summary>
    public interface ICakeTaskInfo
    {
        /// <summary>
        /// Gets the name of the task.
        /// </summary>
        /// <value>The name of the task.</value>
        string Name { get; }

        /// <summary>
        /// Gets the description of the task.
        /// </summary>
        /// <value>The description of the task.</value>
        string Description { get; }

        /// <summary>
        /// Gets the task's dependencies.
        /// </summary>
        /// <value>The task's dependencies.</value>
        IReadOnlyList<string> Dependencies { get; }
    }
}