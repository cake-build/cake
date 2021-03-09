// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        IReadOnlyList<ICakeTaskInfo> Tasks { get; }

        /// <summary>
        /// Raised at the start of setup before any tasks are run.
        /// </summary>
        event EventHandler<SetupEventArgs> Setup;

        /// <summary>
        /// Raised at the end of setup before any tasks are run.
        /// </summary>
        event EventHandler<SetupEventArgs> PostSetup;

        /// <summary>
        /// Raised at the start of teardown after all other tasks have been run.
        /// </summary>
        event EventHandler<TeardownEventArgs> Teardown;

        /// <summary>
        /// Raised at the end of teardown after all other tasks have been run.
        /// </summary>
        event EventHandler<TeardownEventArgs> PostTeardown;

        /// <summary>
        /// Raised at the start of task setup before each task is run.
        /// </summary>
        event EventHandler<TaskSetupEventArgs> TaskSetup;

        /// <summary>
        /// Raised at the end of task setup before each task is run.
        /// </summary>
        event EventHandler<TaskSetupEventArgs> PostTaskSetup;

        /// <summary>
        /// Raised at the start of task teardown after each task has been run.
        /// </summary>
        event EventHandler<TaskTeardownEventArgs> TaskTeardown;

        /// <summary>
        /// Raised at the end of task teardown after each task has been run.
        /// </summary>
        event EventHandler<TaskTeardownEventArgs> PostTaskTeardown;

        /// <summary>
        /// Creates and registers a new Cake task.
        /// </summary>
        /// <param name="name">The name of the task.</param>
        /// <returns>A <see cref="CakeTaskBuilder"/> used to configure the task.</returns>
        CakeTaskBuilder RegisterTask(string name);

        /// <summary>
        /// Allows registration of an action that's executed before any tasks are run.
        /// If setup fails, no tasks will be executed but teardown will be performed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        void RegisterSetupAction(Action<ISetupContext> action);

        /// <summary>
        /// Allows registration of an action that's executed before any tasks are run.
        /// If setup fails, no tasks will be executed but teardown will be performed.
        /// </summary>
        /// <typeparam name="TData">The data type.</typeparam>
        /// <param name="action">The action to be executed.</param>
        void RegisterSetupAction<TData>(Func<ISetupContext, TData> action) where TData : class;

        /// <summary>
        /// Allows registration of an action that's executed after all other tasks have been run.
        /// If a setup action or a task fails with or without recovery, the specified teardown action will still be executed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        void RegisterTeardownAction(Action<ITeardownContext> action);

        /// <summary>
        /// Allows registration of an action that's executed after all other tasks have been run.
        /// If a setup action or a task fails with or without recovery, the specified teardown action will still be executed.
        /// </summary>
        /// <typeparam name="TData">The data type.</typeparam>
        /// <param name="action">The action to be executed.</param>
        void RegisterTeardownAction<TData>(Action<ITeardownContext, TData> action) where TData : class;

        /// <summary>
        /// Runs the specified target using the specified <see cref="IExecutionStrategy"/>.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="strategy">The execution strategy.</param>
        /// <param name="settings">The execution settings.</param>
        /// <returns>The resulting report.</returns>
        Task<CakeReport> RunTargetAsync(ICakeContext context, IExecutionStrategy strategy, ExecutionSettings settings);

        /// <summary>
        /// Allows registration of an action that's executed before each task is run.
        /// If the task setup fails, the task will not be executed but the task's teardown will be performed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        void RegisterTaskSetupAction(Action<ITaskSetupContext> action);

        /// <summary>
        /// Allows registration of an action that's executed before each task is run.
        /// If the task setup fails, the task will not be executed but the task's teardown will be performed.
        /// </summary>
        /// <typeparam name="TData">The data type.</typeparam>
        /// <param name="action">The action to be executed.</param>
        void RegisterTaskSetupAction<TData>(Action<ITaskSetupContext, TData> action) where TData : class;

        /// <summary>
        /// Allows registration of an action that's executed after each task has been run.
        /// If a task setup action or a task fails with or without recovery, the specified task teardown action will still be executed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        void RegisterTaskTeardownAction(Action<ITaskTeardownContext> action);

        /// <summary>
        /// Allows registration of an action that's executed after each task has been run.
        /// If a task setup action or a task fails with or without recovery, the specified task teardown action will still be executed.
        /// </summary>
        /// <typeparam name="TData">The data type.</typeparam>
        /// <param name="action">The action to be executed.</param>
        void RegisterTaskTeardownAction<TData>(Action<ITaskTeardownContext, TData> action) where TData : class;
    }
}