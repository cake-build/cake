// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        IReadOnlyList<ICakeTaskInfo> Tasks { get; }

        /// <summary>
        /// Gets the execution settings.
        /// </summary>
        ExecutionSettings Settings { get; }

        /// <summary>
        /// Registers a new task.
        /// </summary>
        /// <param name="name">The name of the task.</param>
        /// <returns>A <see cref="CakeTaskBuilder"/>.</returns>
        CakeTaskBuilder Task(string name);

        /// <summary>
        /// Allows registration of an action that's executed before any tasks are run.
        /// If setup fails, no tasks will be executed but teardown will be performed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        /// <example>
        /// <code>
        /// Setup(context => {
        ///   context.Log.Information("Hello World!");
        /// });
        /// </code>
        /// </example>
        void Setup(Action<ISetupContext> action);

        /// <summary>
        /// Allows registration of an action that's executed before any tasks are run.
        /// If setup fails, no tasks will be executed but teardown will be performed.
        /// </summary>
        /// <typeparam name="TData">The data type.</typeparam>
        /// <param name="action">The action to be executed.</param>
        /// <example>
        /// <code>
        /// Setup&lt;Foo&gt;(context => {
        ///   return new Foo();
        /// });
        /// </code>
        /// </example>
        void Setup<TData>(Func<ISetupContext, TData> action) where TData : class;

        /// <summary>
        /// Allows registration of an action that's executed after all other tasks have been run.
        /// If a setup action or a task fails with or without recovery, the specified teardown action will still be executed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        /// <example>
        /// <code>
        /// Teardown(context => {
        ///   context.Log.Information("Goodbye World!");
        /// });
        /// </code>
        /// </example>
        void Teardown(Action<ITeardownContext> action);

        /// <summary>
        /// Allows registration of an action that's executed after all other tasks have been run.
        /// If a setup action or a task fails with or without recovery, the specified teardown action will still be executed.
        /// </summary>
        /// <typeparam name="TData">The data type.</typeparam>
        /// <param name="action">The action to be executed.</param>
        /// <example>
        /// <code>
        /// Teardown((context, data) => {
        ///   context.Log.Information("Goodbye {0}!", data.Place);
        /// });
        /// </code>
        /// </example>
        void Teardown<TData>(Action<ITeardownContext, TData> action) where TData : class;

        /// <summary>
        /// Allows registration of an action that's executed before each task is run.
        /// If the task setup fails, its task will not be executed but the task teardown will be performed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        void TaskSetup(Action<ITaskSetupContext> action);

        /// <summary>
        /// Allows registration of an action that's executed before each task is run.
        /// If the task setup fails, its task will not be executed but the task teardown will be performed.
        /// </summary>
        /// <typeparam name="TData">The data type.</typeparam>
        /// <param name="action">The action to be executed.</param>
        void TaskSetup<TData>(Action<ITaskSetupContext, TData> action) where TData : class;

        /// <summary>
        /// Allows registration of an action that's executed after each task has been run.
        /// If a task setup action or a task fails with or without recovery, the specified task teardown action will still be executed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        void TaskTeardown(Action<ITaskTeardownContext> action);

        /// <summary>
        /// Allows registration of an action that's executed after each task has been run.
        /// If a task setup action or a task fails with or without recovery, the specified task teardown action will still be executed.
        /// </summary>
        /// <typeparam name="TData">The data type.</typeparam>
        /// <param name="action">The action to be executed.</param>
        void TaskTeardown<TData>(Action<ITaskTeardownContext, TData> action) where TData : class;

        /// <summary>
        /// Runs the specified target.
        /// </summary>
        /// <param name="target">The target to run.</param>
        /// <returns>The resulting report.</returns>
        CakeReport RunTarget(string target);

        /// <summary>
        /// Runs the specified target.
        /// </summary>
        /// <param name="target">The target to run.</param>
        /// <returns>The resulting report.</returns>
        Task<CakeReport> RunTargetAsync(string target);
    }
}