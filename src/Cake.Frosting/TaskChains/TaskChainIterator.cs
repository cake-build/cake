using System;
using System.Linq;

namespace Cake.Frosting.TaskChains
{
    public class TaskChainIterator
    {
        private readonly TaskChainItem _firstItem;

        public TaskChainIterator(TaskChainItem chainItem)
        {
            ArgumentNullException.ThrowIfNull(chainItem);

            _firstItem = GetChainFirstItem(chainItem);
        }

        static TaskChainItem GetChainFirstItem(TaskChainItem chainItem)
        {
            var current = chainItem;
            while (current.Previous != null)
            {
                current = current.Previous;
            }

            return current;
        }

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
