using System;
using System.Collections.Generic;
using Cake.Core;

namespace Cake.Frosting.TaskChains
{
    public class ChainedTaskConfigurator : DefaultTaskConfigurator
    {
        private readonly TaskChainIterator _chainIterator;

        public ChainedTaskConfigurator(IFrostingContext context, ITaskChainProvider chainProvider) : base(context)
        {
            var chainLastItem = chainProvider.GetChain();
            if (chainLastItem == null)
            {
                throw new InvalidOperationException("No tasks are defined in the chain");
            }
            _chainIterator = new(chainLastItem);
        }

        public override void Configure(IFrostingTask task, CakeTaskBuilder cakeTask)
        {
            base.Configure(task, cakeTask);

            var chainItem = _chainIterator.FindReferencedItem(task);
            if (chainItem != null)
            {
                foreach (var item in GetPreviousItems(chainItem))
                {
                    cakeTask.IsDependentOn(item.GetTaskName());
                }
            }

            if (cakeTask.Task.Name.Equals("Default", StringComparison.OrdinalIgnoreCase))
            {
                var lastTask = _chainIterator.GetLast();
                cakeTask.IsDependentOn(lastTask.GetTaskName());
            }
        }

        IEnumerable<TaskChainItem> GetPreviousItems(TaskChainItem forItem)
        {
            var previous = forItem.Previous;

            if (previous == null)
                yield break;

            if (!previous.IsProxy)
            {
                yield return previous;
                yield break;
            }

            if (previous.IsGroup)
            {
                foreach (var item in previous.EnumerateLeafChain())
                {
                    yield return item;
                }
                yield break;
            }

            var realParent = previous.GetRealParent();
            if (realParent != null)
            {
                foreach (var item in realParent.EnumerateLeafChain())
                {
                    yield return item;
                }
            }
        }
    }
}
