// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cake.Core
{
    /// <summary>
    /// A task that executes a specified delegate.
    /// </summary>
    public sealed class ActionTask : CakeTask
    {
        /// <summary>
        /// Gets the task's actions.
        /// </summary>
        /// <value>The task's actions.</value>
        public List<Func<ICakeContext, Task>> Actions { get; }

        /// <summary>
        /// Gets the task's actions that are run at execution time to additionally populate <see cref="Actions"/>.
        /// </summary>
        /// <value>The task's delayed actions actions.</value>
        public Queue<Action> DelayedActions { get; }

        /// <summary>
        /// Gets a value indicating whether gets the task's state if it will defer exceptions until the end of the task.
        /// </summary>
        /// <value>The task's defer exceptions state.</value>
        public bool DeferExceptions { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionTask"/> class.
        /// </summary>
        /// <param name="name">The name of the task.</param>
        public ActionTask(string name)
            : base(name)
        {
            Actions = new List<Func<ICakeContext, Task>>();
            DelayedActions = new Queue<Action>();
        }

        /// <summary>
        /// Adds an action to the task.
        /// </summary>
        /// <param name="action">The action.</param>
        public void AddAction(Func<ICakeContext, Task> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            Actions.Add(action);
        }

        /// <summary>
        /// Adds a delayed action to the task.
        /// This method will be executed the first time the task is executed.
        /// </summary>
        /// <param name="action">The action.</param>
        public void AddDelayedAction(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            DelayedActions.Enqueue(action);
        }

        /// <summary>
        /// Wait until all actions have executed to report failure.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetDeferExceptions(bool value)
        {
            DeferExceptions = value;
        }

        /// <summary>
        /// Executes the task using the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>Returned Task</returns>
        public override async Task Execute(ICakeContext context)
        {
            while (DelayedActions.Count > 0)
            {
                var delayedDelegate = DelayedActions.Dequeue();
                delayedDelegate();
            }

            var exceptions = new List<Exception>();
            foreach (var action in Actions)
            {
                try
                {
                    await action(context).ConfigureAwait(false);
                }
                catch (Exception e) when (DeferExceptions)
                {
                    exceptions.Add(e);
                }
            }

            if (exceptions.Any())
            {
                if (exceptions.Count == 1)
                {
                    throw exceptions.Single();
                }
                throw new AggregateException("Task failed with following exceptions", exceptions);
            }
        }
    }
}