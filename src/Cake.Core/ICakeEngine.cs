using System;
using System.Collections.Generic;

namespace Cake.Core
{
    /// <summary>
    /// Represents the Cake engine.
    /// </summary>
    public interface ICakeEngine
    {
        /// <summary>
        /// Gets all registered tasks.
        /// </summary>
        /// <value>The registered tasks.</value>
        IReadOnlyList<CakeTask> Tasks { get; }

        /// <summary>
        /// Registers a new task.
        /// </summary>
        /// <param name="name">The name of the task.</param>
        /// <returns>A <see cref="CakeTaskBuilder{ActionTask}"/>.</returns>
        CakeTaskBuilder<ActionTask> RegisterTask(string name);

        /// <summary>
        /// Allows registration of an action that's executed before any tasks are run.
        /// If setup fails, no tasks will be executed but teardown will be performed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        void RegisterSetupAction(Action action);

        /// <summary>
        /// Allows registration of an action that's executed after all other tasks have been run.
        /// If a setup action or a task fails with or without recovery, the specified teardown action will still be executed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        void RegisterTeardownAction(Action action);

        /// <summary>
        /// Gets or sets the name of the target to run.
        /// When null or empty, defaults to "Default".
        /// </summary>
        string Target { get; set; }

        /// <summary>
        /// Runs the specified target using the specified <see cref="IExecutionStrategy"/>.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="strategy">The execution strategy.</param>
        /// <param name="target">The target to run. When let to null, <see cref="Target"/> property is used.</param>
        /// <returns>The resulting report.</returns>
        CakeReport RunTarget(ICakeContext context, IExecutionStrategy strategy, string target = null);
    }
}
