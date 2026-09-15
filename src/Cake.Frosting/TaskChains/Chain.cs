using System;

namespace Cake.Frosting.TaskChains
{
    /// <summary>
    /// The task chain builder.
    /// </summary>
    public static class Chain
    {
        /// <summary>
        /// Adds the task by its type that will be executed first in the build chain.
        /// </summary>
        /// <typeparam name="T">The task type.</typeparam>
        /// <returns>Task configuration.</returns>
        public static TaskChainItem Task<T>() where T : IFrostingTask
        {
            return new TaskChainItem { TaskType = typeof(T) };
        }

        /// <summary>
        /// Adds the task by its name that will be executed first in the build chain.
        /// </summary>
        /// <param name="taskName">Name of the task.</param>
        /// <returns>Task configuration.</returns>
        public static TaskChainItem Task(string taskName)
        {
            return new TaskChainItem { TaskName = taskName };
        }

        /// <summary>
        /// Adds the task that will be executed after the specified task.
        /// </summary>
        /// <typeparam name="T">The type of the task to execute.</typeparam>
        /// <param name="parent">The parent task after which the task will be executed.</param>
        /// <returns>Task configuration.</returns>
        public static TaskChainItem Task<T>(this TaskChainItem parent) where T : IFrostingTask
        {
            var nextItem = new TaskChainItem { TaskType = typeof(T) };
            parent.Next(nextItem);
            return nextItem;
        }

        /// <summary>
        /// Adds the task that will be executed after the specified task.
        /// </summary>
        /// <param name="parent">The parent task after which the task will be executed.</param>
        /// <param name="taskName">Name of the task to execute.</param>
        /// <returns>Task configuration.</returns>
        public static TaskChainItem Task(this TaskChainItem parent, string taskName)
        {
            var nextItem = new TaskChainItem { TaskName = taskName };
            parent.Next(nextItem);
            return nextItem;
        }

        /// <summary>
        /// Adds the group that contains the logically related tasks.
        /// </summary>
        /// <remarks>The tasks in the group are executed sequentially. All the tasks that depend on this group will be executed after the latest
        /// task in this group.</remarks>
        /// <param name="parent">The parent task after which the tasks in the group will be executed.</param>
        /// <param name="groupChain">The callback to configure the tasks in the group.</param>
        /// <returns>The task in the chain.</returns>
        public static TaskChainItem Group(this TaskChainItem parent, Action<TaskChainItem> groupChain)
        {
            return Group(parent, null, groupChain);
        }

        /// <summary>
        /// Adds the group that contains the logically related tasks.
        /// </summary>
        /// <remarks>
        /// The tasks in the group are executed sequentially. All the tasks that depend on this group will be executed after the latest
        /// task in this group.
        /// </remarks>
        /// <param name="parent">The parent task after which the tasks in the group will be executed.</param>
        /// <param name="description">The human-readable group description to simplify understanding the purpose of the group.</param>
        /// <param name="groupChain">The callback to configure the tasks in the group.</param>
        /// <returns>
        /// Task chain configuration.
        /// </returns>
        public static TaskChainItem Group(this TaskChainItem parent, string description, Action<TaskChainItem> groupChain)
        {
            var groupItem = new TaskChainProxyItem
            {
                Description = description
            };
            parent.Next(groupItem);
            groupItem.AddLeafItems(groupChain);

            return groupItem;
        }
    }
}