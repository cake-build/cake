// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using Cake.Core.Diagnostics;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// The script host works as a context for scripts.
    /// </summary>
    public abstract class ScriptHost : IScriptHost
    {
        private readonly ICakeEngine _engine;
        private readonly ICakeContext _context;

        /// <summary>
        /// Gets the engine.
        /// </summary>
        /// <value>The engine.</value>
        protected ICakeEngine Engine
        {
            get { return _engine; }
        }

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>The context.</value>
        public ICakeContext Context
        {
            get { return _context; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptHost"/> class.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <param name="context">The context.</param>
        protected ScriptHost(ICakeEngine engine, ICakeContext context)
        {
            if (engine == null)
            {
                throw new ArgumentNullException("engine");
            }
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            _engine = engine;
            _context = context;
        }

        /// <summary>
        /// Gets all registered tasks.
        /// </summary>
        /// <value>The registered tasks.</value>
        public IReadOnlyList<CakeTask> Tasks
        {
            get { return _engine.Tasks; }
        }

        /// <summary>
        /// Registers a new task.
        /// </summary>
        /// <param name="name">The name of the task.</param>
        /// <returns>A <see cref="CakeTaskBuilder{ActionTask}"/>.</returns>
        public CakeTaskBuilder<ActionTask> Task(string name)
        {
            return _engine.RegisterTask(name);
        }

        /// <summary>
        /// Allows registration of an action that's executed before any tasks are run.
        /// If setup fails, no tasks will be executed but teardown will be performed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        [Obsolete("Please use Setup(Action<ICakeContext>) instead.", false)]
        public void Setup(Action action)
        {
            if (Context != null && Context.Log != null)
            {
                Context.Log.Warning("Please use Setup(Action<ICakeContext>) instead.");
            }
            Setup(context => action());
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
        public void Setup(Action<ICakeContext> action)
        {
            _engine.RegisterSetupAction(action);
        }

        /// <summary>
        /// Allows registration of an action that's executed after all other tasks have been run.
        /// If a setup action or a task fails with or without recovery, the specified teardown action will still be executed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        [Obsolete("Please use Teardown(Action<ICakeContext>) instead.", false)]
        public void Teardown(Action action)
        {
            if (Context != null && Context.Log != null)
            {
                Context.Log.Warning("Please use Teardown(Action<ICakeContext>) instead.");
            }
            Teardown(context => action());
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
        public void Teardown(Action<ICakeContext> action)
        {
            _engine.RegisterTeardownAction(action);
        }

        /// <summary>
        /// Allows registration of an action that's executed before each task is run.
        /// If the task setup fails, its task will not be executed but the task teardown will be performed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        public void TaskSetup(Action<ICakeContext, ITaskSetupContext> action)
        {
            _engine.RegisterTaskSetupAction(action);
        }

        /// <summary>
        /// Allows registration of an action that's executed after each task has been run.
        /// If a task setup action or a task fails with or without recovery, the specified task teardown action will still be executed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        public void TaskTeardown(Action<ICakeContext, ITaskTeardownContext> action)
        {
            _engine.RegisterTaskTeardownAction(action);
        }

        /// <summary>
        /// Runs the specified target.
        /// </summary>
        /// <param name="target">The target to run.</param>
        /// <returns>The resulting report.</returns>
        public abstract CakeReport RunTarget(string target);
    }
}
