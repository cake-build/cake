using System;
using System.Linq;

namespace Cake.Frosting.TaskChains
{
    /// <summary>
    /// Iterates the task chain.
    /// </summary>
    public class TaskChainIterator
    {
        private readonly TaskChainItem _firstItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskChainIterator"/> class.
        /// </summary>
        /// <param name="chainItem">The chain item.</param>
        public TaskChainIterator(TaskChainItem chainItem)
        {
            ArgumentNullException.ThrowIfNull(chainItem);

            _firstItem = GetChainFirstItem(chainItem);
        }

        private static TaskChainItem GetChainFirstItem(TaskChainItem chainItem)
        {
            var current = chainItem;
            while (current.Previous != null)
            {
                current = current.Previous;
            }

            return current;
        }

        /// <summary>
        /// Finds the task configuration that references the specified task.
        /// </summary>
        /// <param name="task">The task which reference to find.</param>
        /// <returns>Task configuration or null if not found.</returns>
        public TaskChainItem FindReferencedItem(IFrostingTask task)
        {
            var currentItem = _firstItem;
            while (currentItem != null)
            {
                foreach (var item in currentItem.EnumerateLeafChain())
                {
                    if (item.Equals(task))
                    {
                        return item;
                    }
                }

                currentItem = currentItem.Next;
            }

            return null;
        }

        /// <summary>
        /// Gets the last task in the chain.
        /// </summary>
        /// <returns>The last task in the chain.</returns>
        public TaskChainItem GetLast()
        {
            return FindLastItem(_firstItem);
        }

        private TaskChainItem FindLastItem(TaskChainItem start)
        {
            var currentItem = start;
            TaskChainItem lastFoundItem = null;

            while (currentItem != null)
            {
                lastFoundItem = currentItem.EnumerateLeafChain().LastOrDefault() ?? lastFoundItem;
                if (currentItem.Next == null)
                {
                    break;
                }
                currentItem = currentItem.Next;
            }

            return lastFoundItem;
        }
    }
}
