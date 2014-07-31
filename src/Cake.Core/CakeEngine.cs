using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Cake.Core.Diagnostics;
using Cake.Core.Graph;
using Cake.Core.IO;

namespace Cake.Core
{
    /// <summary>
    /// The Cake execution engine.
    /// </summary>
    public sealed class CakeEngine : ICakeEngine
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly IGlobber _globber;
        private readonly ICakeLog _log;
        private readonly ICakeArguments _arguments;
        private readonly IProcessRunner _processRunner;
        private readonly List<CakeTask> _tasks;

        /// <summary>
        /// Gets the file system.
        /// </summary>
        /// <value>The file system.</value>
        public IFileSystem FileSystem
        {
            get { return _fileSystem; }
        }

        /// <summary>
        /// Gets the environment.
        /// </summary>
        /// <value>The environment.</value>
        public ICakeEnvironment Environment
        {
            get { return _environment; }
        }

        /// <summary>
        /// Gets all registered tasks.
        /// </summary>
        /// <value>The registered tasks.</value>
        public IReadOnlyList<CakeTask> Tasks
        {
            get { return _tasks; }
        }

        /// <summary>
        /// Gets the globber.
        /// </summary>
        /// <value>The globber.</value>
        public IGlobber Globber
        {
            get { return _globber; }
        }

        /// <summary>
        /// Gets the log.
        /// </summary>
        /// <value>The log.</value>
        public ICakeLog Log
        {
            get { return _log; }
        }

        /// <summary>
        /// Gets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public ICakeArguments Arguments
        {
            get { return _arguments; }
        }

        /// <summary>
        /// Gets the process runner.
        /// </summary>
        /// <value>The process runner.</value>
        public IProcessRunner ProcessRunner
        {
            get { return _processRunner; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeEngine"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="log">The log.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="globber">The globber.</param>
        /// <param name="processRunner">The process runner.</param>
        public CakeEngine(IFileSystem fileSystem, ICakeEnvironment environment, ICakeLog log, 
            ICakeArguments arguments, IGlobber globber, IProcessRunner processRunner)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem");
            }
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }
            if (arguments == null)
            {
                throw new ArgumentNullException("arguments");
            }
            if (globber == null)
            {
                throw new ArgumentNullException("globber");
            }
            if (processRunner == null)
            {
                throw new ArgumentNullException("processRunner");
            }
            _fileSystem = fileSystem;
            _environment = environment;
            _log = log;            
            _arguments = arguments;
            _globber = globber;
            _processRunner = processRunner;
            _tasks = new List<CakeTask>();
        }

        /// <summary>
        /// Creates and registers a new task.
        /// </summary>
        /// <typeparam name="T">The task type.</typeparam>
        /// <returns>
        /// A <see cref="CakeTaskBuilder{T}"/> used to configure the task.
        /// </returns>
        public CakeTaskBuilder<T> Build<T>()
            where T : CakeTask, new()
        {
            var task = new T();
            _tasks.Add(task);
            var builder = new CakeTaskBuilder<T>(task);
            return builder;
        }

        /// <summary>
        /// Creates and registers a new <see cref="ActionTask"/>.
        /// </summary>
        /// <param name="name">The name of the task.</param>
        /// <returns>
        /// A <see cref="CakeTaskBuilder{T}"/> used to configure the task.
        /// </returns>
        public CakeTaskBuilder<ActionTask> Task(string name)
        {
            if (_tasks.Any(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                const string format = "Another task with the name '{0}' has already been added.";
                throw new CakeException(string.Format(format, name));
            }
            var task = new ActionTask(name);
            _tasks.Add(task);
            return new CakeTaskBuilder<ActionTask>(task);
        }

        /// <summary>
        /// Runs the specified target.
        /// </summary>
        /// <param name="target">The target to run.</param>
        /// <returns>The resulting report.</returns>
        public CakeReport RunTarget(string target)
        {
            var graph = CakeGraphBuilder.Build(_tasks);

            // Make sure target exist.
            if (!graph.Exist(target))
            {
                const string format = "The target '{0}' was not found.";
                throw new CakeException(string.Format(format, target));
            }

            var stopWatch = new Stopwatch();
            var report = new CakeReport();

            foreach (var task in graph.Traverse(target))
            {
                var taskNode = _tasks.FirstOrDefault(x => x.Name.Equals(task, StringComparison.OrdinalIgnoreCase));
                Debug.Assert(taskNode != null, "Node should not be null.");

                if (ShouldTaskExecute(taskNode))
                {
                    _log.Verbose("Executing task: {0}", taskNode.Name);

                    ExecuteTask(stopWatch, taskNode, report);

                    _log.Verbose("Finished executing task: {0}", taskNode.Name);
                }
            }

            return report;
        }

        private static bool ShouldTaskExecute(CakeTask task)
        {
            foreach (var criteria in task.Criterias)
            {
                if (!criteria())
                {
                    return false;
                }
            }
            return true;
        }

        private void ExecuteTask(Stopwatch stopWatch, CakeTask task, CakeReport report)
        {
            // Reset the stop watch.
            stopWatch.Reset();
            stopWatch.Start();
            
            try
            {
                // Execute the task.
                task.Execute(this);
            }
            catch (Exception ex)
            {
                _log.Error("An error occured in task {0}.", ex.Message);
                if (!task.ContinueOnError)
                {
                    throw;   
                }                
            }            

            // Add the task results to the report.
            report.Add(task.Name, stopWatch.Elapsed);
        }
    }
}
