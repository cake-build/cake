// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
        void RegisterSetupAction(Action<ICakeContext> action);

        /// <summary>
        /// Raised during setup before any tasks are run.
        /// </summary>
        event EventHandler<SetupEventArgs> Setup;

        /// <summary>
        /// Allows registration of an action that's executed after all other tasks have been run.
        /// If a setup action or a task fails with or without recovery, the specified teardown action will still be executed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        void RegisterTeardownAction(Action<ITeardownContext> action);

        /// <summary>
        /// Raised during teardown after all other tasks have been run.
        /// </summary>
        event EventHandler<TeardownEventArgs> Teardown;

        /// <summary>
        /// Runs the specified target using the specified <see cref="IExecutionStrategy"/>.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="strategy">The execution strategy.</param>
        /// <param name="target">The target to run.</param>
        /// <returns>The resulting report.</returns>
        CakeReport RunTarget(ICakeContext context, IExecutionStrategy strategy, string target);

        /// <summary>
        /// Allows registration of an action that's executed before each task is run.
        /// If the task setup fails, the task will not be executed but the task's teardown will be performed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        void RegisterTaskSetupAction(Action<ITaskSetupContext> action);

        /// <summary>
        /// Raised before each task is run.
        /// </summary>
        event EventHandler<TaskSetupEventArgs> TaskSetup;

        /// <summary>
        /// Allows registration of an action that's executed after each task has been run.
        /// If a task setup action or a task fails with or without recovery, the specified task teardown action will still be executed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        void RegisterTaskTeardownAction(Action<ITaskTeardownContext> action);

        /// <summary>
        /// Raised after each task has been run.
        /// </summary>
        event EventHandler<TaskTeardownEventArgs> TaskTeardown;
    }
}