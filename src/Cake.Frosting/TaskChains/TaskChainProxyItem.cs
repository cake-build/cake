using System;
using System.Collections.Generic;

namespace Cake.Frosting.TaskChains
{
    /// <summary>
    /// A proxy-task task type that does nothing but aggregates other tasks.
    /// </summary>
    public class TaskChainProxyItem : TaskChainItem
    {
        /// <summary>
        /// Gets or sets the human-readable description of the tasks in the group.
        /// </summary>
        public string Description { get; set; }

        private TaskChainItem _leaf;

        /// <summary>
        /// Gets a value indicating whether the task is a proxy-link between the real tasks. The proxy behaves as a group.
        /// </summary>
        public override bool IsProxy => true;

        /// <summary>
        /// Gets the child tasks.
        /// </summary>
        protected override TaskChainItem Leaf => _leaf;

        /// <summary>
        /// Gets the task that owns this proxy.
        /// </summary>
        public TaskChainProxyItem Owner => GetOwner(this);

        /// <summary>
        /// Adds the child tasks.
        /// </summary>
        /// <param name="leafChainAction">An action that configures the child tasks.</param>
        public void AddLeafItems(Action<TaskChainItem> leafChainAction)
        {
            _leaf = new TaskChainProxyItem
            {
                Previous = this
            };
            leafChainAction(Leaf);
        }

        /// <summary>
        /// Enumerates the child tasks in the chain.
        /// </summary>
        /// <returns>
        /// The child tasks.
        /// </returns>
        public override IEnumerable<TaskChainItem> EnumerateLeafChain()
        {
            var nextChild = Owner.Leaf.Next;
            while (nextChild != null)
            {
                foreach (var item in nextChild.EnumerateLeafChain())
                {
                    yield return item;
                }

                nextChild = nextChild.Next;
            }
        }

        private static TaskChainProxyItem GetOwner(TaskChainItem start)
        {
            var current = start;
            while (current != null && !current.IsGroup)
            {
                current = current.Previous;
            }
            return (TaskChainProxyItem)current;
        }
    }
}