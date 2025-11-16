using System;
using System.Collections.Generic;
using Cake.Core;

namespace Cake.Frosting.TaskChains
{
    /// <summary>
    /// Applies the provided chain of tasks to the build.
    /// </summary>
    public class ChainedTaskConfigurator : DefaultTaskConfigurator
    {
        private readonly TaskChainIterator _chainIterator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChainedTaskConfigurator"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="chainProvider">The chain provider.</param>
        public ChainedTaskConfigurator(IFrostingContext context, ITaskChainProvider chainProvider) : base(context)
        {
            var chainLastItem = chainProvider.GetChain();
            if (chainLastItem == null)
            {
                throw new InvalidOperationException("No tasks are defined in the chain");
            }
            _chainIterator = new (chainLastItem);
        }

        /// <summary>
        /// Configures the specific task after it was added to the execution engine.
        /// </summary>
        /// <param name="task">The task class instance.</param>
        /// <param name="cakeTask">The task configuration in Cake engine.</param>
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

        /// <summary>
        /// Gets all the tasks that are higher in the hierarchy of the provided task.
        /// </summary>
        /// <param name="forItem">For child task which parents to find.</param>
        /// <returns>List of parent tasks.</returns>
        private IEnumerable<TaskChainItem> GetPreviousItems(TaskChainItem forItem)
        {
            var previous = forItem.Previous;

            if (previous == null)
            {
                yield break;
            }

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
