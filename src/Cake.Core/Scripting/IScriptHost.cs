using System;
using System.Collections.Generic;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// Represents a script host that works as a context for scripts.
    /// </summary>
    public interface IScriptHost
    {
        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>The context.</value>
        ICakeContext Context { get; }

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
        CakeTaskBuilder<ActionTask> Task(string name);

        /// <summary>
        /// Allows registration of an action that's executed before any tasks are run.
        /// If setup fails, no tasks will be executed but teardown will be performed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        void Setup(Action action);

        /// <summary>
        /// Allows registration of an action that's executed after all other tasks have been run.
        /// If a setup action or a task fails with or without recovery, the specified teardown action will still be executed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        void Teardown(Action action);

        /// <summary>
        /// Runs the specified target.
        /// </summary>
        /// <param name="target">The target to run.</param>
        /// <returns>The resulting report.</returns>
        [Obsolete("Use SetTarget instead.")]
        CakeReport RunTarget(string target);

        /// <summary>
        /// Declares the specified target as the running one.
        /// </summary>
        /// <param name="target">The target to run.</param>
        void SetTarget(string target);
    }
}