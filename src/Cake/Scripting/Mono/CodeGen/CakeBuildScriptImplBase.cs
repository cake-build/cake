// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Scripting;

namespace Cake.Scripting.Mono.CodeGen
{
    /// <summary>
    /// Base class used for the Mono script code generation
    /// </summary>
    public class CakeBuildScriptImplBase : IScriptHost
    {
        /// <summary>
        /// Gets the script host.
        /// </summary>
        /// <value>The script host.</value>
        public IScriptHost ScriptHost { get; private set; }

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>The context.</value>
        public ICakeContext Context
        {
            get { return ScriptHost.Context; }
        }

        /// <summary>
        /// Gets all registered tasks.
        /// </summary>
        /// <value>The registered tasks.</value>
        public IReadOnlyList<CakeTask> Tasks
        {
            get { return ScriptHost.Tasks; }
        }

        /// <summary>
        /// Registers a new task.
        /// </summary>
        /// <param name="name">The name of the task.</param>
        /// <returns>A <see cref="CakeTaskBuilder{ActionTask}"/>.</returns>
        public CakeTaskBuilder<ActionTask> Task(string name)
        {
            return ScriptHost.Task(name);
        }

        /// <summary>
        /// Allows registration of an action that's executed before any tasks are run.
        /// If setup fails, no tasks will be executed but teardown will be performed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        [Obsolete("Please use Setup(Action<ICakeContext>) instead.", false)]
        public void Setup(Action action)
        {
            ScriptHost.Setup(context => action());
        }

        /// <summary>
        /// Allows registration of an action that's executed before any tasks are run.
        /// If setup fails, no tasks will be executed but teardown will be performed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        public void Setup(Action<ICakeContext> action)
        {
            ScriptHost.Setup(action);
        }

        /// <summary>
        /// Allows registration of an action that's executed after all other tasks have been run.
        /// If a setup action or a task fails with or without recovery, the specified teardown action will still be executed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        [Obsolete("Please use Teardown(Action<ICakeContext>) instead.", false)]
        public void Teardown(Action action)
        {
            ScriptHost.Teardown(context => action());
        }

        /// <summary>
        /// Allows registration of an action that's executed after all other tasks have been run.
        /// If a setup action or a task fails with or without recovery, the specified teardown action will still be executed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        public void Teardown(Action<ICakeContext> action)
        {
            ScriptHost.Teardown(action);
        }

        /// <summary>
        /// Allows registration of an action that's executed before each task is run.
        /// If the task setup fails, its task will not be executed but the task teardown will be performed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        public void TaskSetup(Action<ICakeContext, ITaskSetupContext> action)
        {
            ScriptHost.TaskSetup(action);
        }

        /// <summary>
        /// Allows registration of an action that's executed after each task has been run.
        /// If a task setup action or a task fails with or without recovery, the specified task teardown action will still be executed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        public void TaskTeardown(Action<ICakeContext, ITaskTeardownContext> action)
        {
            ScriptHost.TaskTeardown(action);
        }

        /// <summary>
        /// Runs the specified target.
        /// </summary>
        /// <param name="target">The target to run.</param>
        /// <returns>The resulting report.</returns>
        public CakeReport RunTarget(string target)
        {
            return ScriptHost.RunTarget(target);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeBuildScriptImplBase"/> class.
        /// </summary>
        /// <param name="scriptHost">The script host.</param>
        public CakeBuildScriptImplBase(IScriptHost scriptHost)
        {
            ScriptHost = scriptHost;
        }
    }
}
