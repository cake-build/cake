// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cake.Core.Diagnostics;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// The script host works as a context for scripts.
    /// </summary>
    public abstract class ScriptHost : IScriptHost
    {
        /// <summary>
        /// Gets the engine.
        /// </summary>
        /// <value>The engine.</value>
        protected ICakeEngine Engine { get; }

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>The context.</value>
        public ICakeContext Context { get; }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>The settings.</value>
        public ExecutionSettings Settings { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptHost"/> class.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <param name="context">The context.</param>
        protected ScriptHost(ICakeEngine engine, ICakeContext context)
        {
            if (engine == null)
            {
                throw new ArgumentNullException(nameof(engine));
            }
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            Engine = engine;
            Context = context;
            Settings = new ExecutionSettings();
        }

        /// <summary>
        /// Gets all registered tasks.
        /// </summary>
        /// <value>The registered tasks.</value>
        public IReadOnlyList<ICakeTaskInfo> Tasks => Engine.Tasks;

        /// <summary>
        /// Registers a new task.
        /// </summary>
        /// <param name="name">The name of the task.</param>
        /// <returns>A <see cref="CakeTaskBuilder"/>.</returns>
        public CakeTaskBuilder Task(string name)
        {
            return Engine.RegisterTask(name);
        }

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
        public void Setup(Action<ISetupContext> action)
        {
            Engine.RegisterSetupAction(action);
        }

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
        public void Setup<TData>(Func<ISetupContext, TData> action)
            where TData : class
        {
            Engine.RegisterSetupAction(action);
        }

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
        public void Teardown(Action<ITeardownContext> action)
        {
            Engine.RegisterTeardownAction(action);
        }

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
        public void Teardown<TData>(Action<ITeardownContext, TData> action) where TData : class
        {
            Engine.RegisterTeardownAction(action);
        }

        /// <summary>
        /// Allows registration of an action that's executed before each task is run.
        /// If the task setup fails, its task will not be executed but the task teardown will be performed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        public void TaskSetup(Action<ITaskSetupContext> action)
        {
            Engine.RegisterTaskSetupAction(action);
        }

        /// <summary>
        /// Allows registration of an action that's executed before each task is run.
        /// If the task setup fails, its task will not be executed but the task teardown will be performed.
        /// </summary>
        /// <typeparam name="TData">The data type.</typeparam>
        /// <param name="action">The action to be executed.</param>
        public void TaskSetup<TData>(Action<ITaskSetupContext, TData> action) where TData : class
        {
            Engine.RegisterTaskSetupAction(action);
        }

        /// <summary>
        /// Allows registration of an action that's executed after each task has been run.
        /// If a task setup action or a task fails with or without recovery, the specified task teardown action will still be executed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        public void TaskTeardown(Action<ITaskTeardownContext> action)
        {
            Engine.RegisterTaskTeardownAction(action);
        }

        /// <summary>
        /// Allows registration of an action that's executed after each task has been run.
        /// If a task setup action or a task fails with or without recovery, the specified task teardown action will still be executed.
        /// </summary>
        /// <typeparam name="TData">The data type.</typeparam>
        /// <param name="action">The action to be executed.</param>
        public void TaskTeardown<TData>(Action<ITaskTeardownContext, TData> action) where TData : class
        {
            Engine.RegisterTaskTeardownAction(action);
        }

        /// <summary>
        /// Runs the specified target.
        /// </summary>
        /// <param name="target">The target to run.</param>
        /// <returns>The resulting report.</returns>
        public CakeReport RunTarget(string target)
        {
            return RunTargetAsync(target).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Runs the specified target.
        /// </summary>
        /// <param name="target">The target to run.</param>
        /// <returns>The resulting report.</returns>
        public abstract Task<CakeReport> RunTargetAsync(string target);
    }
}