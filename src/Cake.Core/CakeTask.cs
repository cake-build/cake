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
    /// A <see cref="CakeTask"/> represents a unit of work.
    /// </summary>
    public sealed class CakeTask : ICakeTaskInfo
    {
        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public string Description { get; set; }

        /// <summary>
        /// Gets the task's dependencies.
        /// </summary>
        /// <value>The task's dependencies.</value>
        public List<CakeTaskDependency> Dependencies { get; }

        /// <summary>
        /// Gets the tasks that the task want to be a dependency of.
        /// </summary>
        /// <value>The tasks that the task want to be a dependency of.</value>
        public List<CakeTaskDependency> Dependees { get; }

        /// <summary>
        /// Gets the task's criterias.
        /// </summary>
        /// <value>The task's criterias.</value>
        public List<CakeTaskCriteria> Criterias { get; }

        /// <summary>
        /// Gets or sets the error handler.
        /// </summary>
        /// <value>The error handler.</value>
        public Func<Exception, ICakeContext, Task> ErrorHandler { get; set; }

        /// <summary>
        /// Gets or sets the error reporter.
        /// </summary>
        public Func<Exception, Task> ErrorReporter { get; set; }

        /// <summary>
        /// Gets or sets the finally handler.
        /// </summary>
        public Func<Task> FinallyHandler { get; set; }

        /// <summary>
        /// Gets the task's actions.
        /// </summary>
        /// <value>The task's actions.</value>
        public List<Func<ICakeContext, Task>> Actions { get; }

        /// <summary>
        /// Gets the task's actions that are run at execution time to additionally populate <see cref="Actions"/>.
        /// </summary>
        /// <value>The task's delayed actions actions.</value>
        public Queue<Action<ICakeContext>> DelayedActions { get; }

        /// <summary>
        /// Gets or sets a value indicating whether gets the task's state if it will defer exceptions until the end of the task.
        /// </summary>
        /// <value>The task's defer exceptions state.</value>
        public bool DeferExceptions { get; set; }

        IReadOnlyList<CakeTaskDependency> ICakeTaskInfo.Dependencies => Dependencies;
        IReadOnlyList<CakeTaskDependency> ICakeTaskInfo.Dependees => Dependees;

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeTask"/> class.
        /// </summary>
        /// <param name="name">The name of the task.</param>
        public CakeTask(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Task name cannot be empty.");
            }

            Name = name;
            Dependencies = new List<CakeTaskDependency>();
            Dependees = new List<CakeTaskDependency>();
            Criterias = new List<CakeTaskCriteria>();
            Actions = new List<Func<ICakeContext, Task>>();
            DelayedActions = new Queue<Action<ICakeContext>>();
        }

        /// <summary>
        /// Executes the task using the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>Returned Task.</returns>
        public async Task Execute(ICakeContext context)
        {
            while (DelayedActions.Count > 0)
            {
                var delayedDelegate = DelayedActions.Dequeue();
                delayedDelegate(context);
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