using System;
using System.Collections.Generic;
using Cake.Frosting.Internal;

namespace Cake.Frosting.TaskChains
{
    /// <summary>
    /// Configuration of the task in the chain.
    /// </summary>
    public class TaskChainItem
    {
        /// <summary>
        /// Gets or sets the type of the task to execute in the chain.
        /// </summary>
        public Type TaskType { get; set; }

        /// <summary>
        /// Gets or sets the name of the task to execute in the chain.
        /// </summary>
        public string TaskName { get; set; }

        /// <summary>
        /// Gets or sets the next task to execute.
        /// </summary>
        public TaskChainItem Next { get; set; }

        /// <summary>
        /// Gets or sets the previous task to execute.
        /// </summary>
        public TaskChainItem Previous { get; set; }

        /// <summary>
        /// Gets the child tasks.
        /// </summary>
        protected virtual TaskChainItem Leaf => null;

        /// <summary>
        /// Gets a value indicating whether this task is a group of tasks.
        /// </summary>
        /// <remarks>The group of tasks organizes other tasks to run sequentially and ensures that subsequent tasks wait until all grouped
        /// tasks are finished.</remarks>
        public bool IsGroup => Leaf != null;

        /// <summary>
        /// Gets a value indicating whether the task is a proxy-link between the real tasks. The proxy behaves as a group.
        /// </summary>
        public virtual bool IsProxy => false;

        /// <summary>
        /// Enumerates the child tasks in the chain.
        /// </summary>
        /// <returns>The child tasks.</returns>
        public virtual IEnumerable<TaskChainItem> EnumerateLeafChain()
        {
            yield return this;
        }

        /// <summary>
        /// Compares the task with the specified task.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <returns>true if the task is equal to the specified task; otherwise, false.</returns>
        public bool Equals(IFrostingTask task)
        {
            return (TaskType != null && TaskType == task.GetType()) ||
                   (TaskName != null && TaskName.Equals(task.GetType().GetTaskName(), StringComparison.OrdinalIgnoreCase));
        }
    }
}
