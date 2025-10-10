using Cake.Frosting.Internal;

namespace Cake.Frosting.TaskChains
{
    /// <summary>
    /// Tasks chain extensions.
    /// </summary>
    public static class TaskChainExtensions
    {
        /// <summary>
        /// Sets the next task to be executed in the chain.
        /// </summary>
        /// <param name="parent">The task whose next task to set.</param>
        /// <param name="next">The next task to execute.</param>
        public static void Next(this TaskChainItem parent, TaskChainItem next)
        {
            parent.Next = next;
            next.Previous = parent;
        }

        /// <summary>
        /// Gets the specified task name.
        /// </summary>
        /// <param name="item">The task which name to get.</param>
        /// <returns>Task name.</returns>
        public static string GetTaskName(this TaskChainItem item)
        {
            return item.TaskName ?? item.TaskType.GetTaskName();
        }

        /// <summary>
        /// Finds the first real parent tasks skipping all proxies.
        /// </summary>
        /// <param name="item">The child task which parent to find.</param>
        /// <returns>The parent task or null.</returns>
        public static TaskChainItem GetRealParent(this TaskChainItem item)
        {
            var current = item.Previous;
            while (current?.IsProxy == true)
            {
                var nextPrevious = current.Previous;
                if (current.IsGroup && nextPrevious?.IsGroup == true)
                {
                    return nextPrevious;
                }
                current = nextPrevious;
            }
            return current;
        }
    }
}