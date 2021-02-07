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

        /// <inheritdoc/>
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
            Engine = engine ?? throw new ArgumentNullException(nameof(engine));
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Settings = new ExecutionSettings();
        }

        /// <inheritdoc/>
        public IReadOnlyList<ICakeTaskInfo> Tasks => Engine.Tasks;

        /// <inheritdoc/>
        public CakeTaskBuilder Task(string name)
        {
            return Engine.RegisterTask(name);
        }

        /// <inheritdoc/>
        public void Setup(Action<ISetupContext> action)
        {
            Engine.RegisterSetupAction(action);
        }

        /// <inheritdoc/>
        public void Setup<TData>(Func<ISetupContext, TData> action)
            where TData : class
        {
            Engine.RegisterSetupAction(action);
        }

        /// <inheritdoc/>
        public void Teardown(Action<ITeardownContext> action)
        {
            Engine.RegisterTeardownAction(action);
        }

        /// <inheritdoc/>
        public void Teardown<TData>(Action<ITeardownContext, TData> action) where TData : class
        {
            Engine.RegisterTeardownAction(action);
        }

        /// <inheritdoc/>
        public void TaskSetup(Action<ITaskSetupContext> action)
        {
            Engine.RegisterTaskSetupAction(action);
        }

        /// <inheritdoc/>
        public void TaskSetup<TData>(Action<ITaskSetupContext, TData> action) where TData : class
        {
            Engine.RegisterTaskSetupAction(action);
        }

        /// <inheritdoc/>
        public void TaskTeardown(Action<ITaskTeardownContext> action)
        {
            Engine.RegisterTaskTeardownAction(action);
        }

        /// <inheritdoc/>
        public void TaskTeardown<TData>(Action<ITaskTeardownContext, TData> action) where TData : class
        {
            Engine.RegisterTaskTeardownAction(action);
        }

        /// <inheritdoc/>
        public CakeReport RunTarget(string target)
        {
            return RunTargetAsync(target).GetAwaiter().GetResult();
        }

        /// <inheritdoc/>
        public abstract Task<CakeReport> RunTargetAsync(string target);
    }
}