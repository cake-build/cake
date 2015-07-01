using System;
using Cake.Core.Scripting;
using Cake.Core;
using System.Collections.Generic;

namespace Cake
{
    public static class MonoScriptHostProxy
    {
        public static IScriptHost ScriptHost { get; set; }

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>The context.</value>
        public static ICakeContext Context { get { return ScriptHost.Context; } }

        /// <summary>
        /// Gets all registered tasks.
        /// </summary>
        /// <value>The registered tasks.</value>
        public static IReadOnlyList<CakeTask> Tasks { get { return ScriptHost.Tasks; } }

        /// <summary>
        /// Registers a new task.
        /// </summary>
        /// <param name="name">The name of the task.</param>
        /// <returns>A <see cref="CakeTaskBuilder{ActionTask}"/>.</returns>
        public static CakeTaskBuilder<ActionTask> Task(string name)
        {
            return ScriptHost.Task (name);
        }

        /// <summary>
        /// Allows registration of an action that's executed before any tasks are run.
        /// If setup fails, no tasks will be executed but teardown will be performed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        public static void Setup (Action action)
        {
            ScriptHost.Setup (action);
        }

        /// <summary>
        /// Allows registration of an action that's executed after all other tasks have been run.
        /// If a setup action or a task fails with or without recovery, the specified teardown action will still be executed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        public static void Teardown(Action action)
        {
            ScriptHost.Teardown (action);
        }

        /// <summary>
        /// Runs the specified target.
        /// </summary>
        /// <param name="target">The target to run.</param>
        /// <returns>The resulting report.</returns>
        public static CakeReport RunTarget(string target)
        {
            return ScriptHost.RunTarget (target);
        }
    }
}

