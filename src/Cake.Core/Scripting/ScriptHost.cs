using System;
using System.Collections.Generic;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// The script host that works as a context for the scripts.
    /// </summary>
    public abstract class ScriptHost : IScriptHost
    {
        private readonly ICakeEngine _engine;

        /// <summary>
        /// Gets the file system.
        /// </summary>
        /// <value>The file system.</value>
        public IFileSystem FileSystem
        {
            get { return _engine.FileSystem; }
        }

        /// <summary>
        /// Gets the environment.
        /// </summary>
        /// <value>The environment.</value>
        public ICakeEnvironment Environment
        {
            get { return _engine.Environment; }
        }

        /// <summary>
        /// Gets the globber.
        /// </summary>
        /// <value>The globber.</value>
        public IGlobber Globber
        {
            get { return _engine.Globber; }
        }

        /// <summary>
        /// Gets the log.
        /// </summary>
        /// <value>The log.</value>
        public ICakeLog Log
        {
            get { return _engine.Log; }
        }

        /// <summary>
        /// Gets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public ICakeArguments Arguments
        {
            get { return _engine.Arguments; }
        }

        /// <summary>
        /// Gets the process runner.
        /// </summary>
        /// <value>The process runner.</value>
        public IProcessRunner ProcessRunner
        {
            get { return _engine.ProcessRunner; }
        }

        /// <summary>
        /// Gets the engine.
        /// </summary>
        /// <value>The engine.</value>
        protected ICakeEngine Engine
        {
            get { return _engine; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptHost"/> class.
        /// </summary>
        /// <param name="engine">The engine.</param>
        protected ScriptHost(ICakeEngine engine)
        {
            if (engine == null)
            {
                throw new ArgumentNullException("engine");
            }
            _engine = engine;
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
            return _engine.Task(name);
        }

        /// <summary>
        /// Gets the <see cref="ICakeContext"/>.
        /// </summary>
        /// <returns>The context.</returns>
        public ICakeContext GetContext()
        {
            return this;
        }

        /// <summary>
        /// Allows registration of an action that's executed before any tasks are run.
        /// If setup fails, no tasks will be executed but teardown will be performed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        public void Setup(Action action)
        {
            _engine.Setup(action);
        }

        /// <summary>
        /// Allows registration of an action that's executed after all other tasks have been run.
        /// If a setup action or a task fails with or without recovery, the specified teardown action will still be executed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        public void Teardown(Action action)
        {
            _engine.Teardown(action);
        }

        /// <summary>
        /// Runs the specified target.
        /// </summary>
        /// <param name="target">The target to run.</param>
        /// <returns>The resulting report.</returns>
        public abstract CakeReport RunTarget(string target);
    }
}
