using Cake.Frosting.Internal;

namespace Cake.Frosting.TaskChains
{
    public static class TaskChainExtensions
    {
        public static void Next(this TaskChainItem parent, TaskChainItem next)
        {
            parent.Next = next;
            next.Previous = parent;
        }

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