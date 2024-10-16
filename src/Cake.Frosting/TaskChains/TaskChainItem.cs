using System;
using System.Collections.Generic;
using Cake.Frosting.Internal;

namespace Cake.Frosting.TaskChains
{
    public class TaskChainItem
    {
        public Type TaskType { get; set; }
        public string TaskName { get; set; }
        public TaskChainItem Next { get; set; }
        public TaskChainItem Previous { get; set; }
        protected virtual TaskChainItem Leaf => null;
        public bool IsGroup => Leaf != null;
        public virtual bool IsProxy => false;

        public virtual IEnumerable<TaskChainItem> EnumerateLeafChain()
        {
            yield return this;
        }

        public bool Equals(IFrostingTask task)
        {
            return (TaskType != null && TaskType == task.GetType()) ||
                   (TaskName != null && TaskName.Equals(task.GetType().GetTaskName(), StringComparison.OrdinalIgnoreCase));
        }
    }

    public class TaskChainProxyItem : TaskChainItem
    {
        public string Description { get; set; }

        TaskChainItem _leaf;

        public override bool IsProxy => true;

        protected override TaskChainItem Leaf => _leaf;

        public TaskChainProxyItem Owner => GetOwner(this);

        public void AddLeafItems(Action<TaskChainItem> leafChainAction)
        {
            _leaf = new TaskChainProxyItem
            {
                Previous = this
            };
            leafChainAction(Leaf);
        }

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

        static TaskChainProxyItem GetOwner(TaskChainItem start)
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
