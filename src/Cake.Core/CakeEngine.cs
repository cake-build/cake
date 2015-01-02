using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
        private readonly ILookup<string, IToolResolver> _toolResolverLookup;
        private Action _setupAction;
        private Action _teardownAction;

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
        /// Gets resolver by tool name
        /// </summary>
        /// <param name="toolName">resolver tool name</param>
        /// <returns>IToolResolver for tool</returns>
        public IToolResolver GetToolResolver(string toolName)
        {
            var toolResolver = _toolResolverLookup[toolName].FirstOrDefault();
            if (toolResolver == null)
            {
                throw new CakeException(string.Format(CultureInfo.InvariantCulture, "Failed to resolve tool: {0}", toolName));
            }
            return toolResolver;
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
        /// <param name="toolResolvers">The tool resolvers.</param>
        public CakeEngine(IFileSystem fileSystem, ICakeEnvironment environment, ICakeLog log, 
            ICakeArguments arguments, IGlobber globber, IProcessRunner processRunner,
            IEnumerable<IToolResolver> toolResolvers)
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
            if (processRunner == null)
            {
                throw new ArgumentNullException("toolResolvers");
            }
            _fileSystem = fileSystem;
            _environment = environment;
            _log = log;            
            _arguments = arguments;
            _globber = globber;
            _processRunner = processRunner;
            _tasks = new List<CakeTask>();
            _toolResolverLookup =toolResolvers.ToLookup(
                key=>key.Name,
                value=>value,
                StringComparer.OrdinalIgnoreCase
                );
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
                throw new CakeException(string.Format(CultureInfo.InvariantCulture, format, name));
            }
            var task = new ActionTask(name);
            _tasks.Add(task);
            return new CakeTaskBuilder<ActionTask>(task);
        }

        /// <summary>
        /// Allows registration of an action that's executed before any tasks are run.
        /// If setup fails, no tasks will be executed but teardown will be performed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        public void Setup(Action action)
        {
            _setupAction = action;
        }

        /// <summary>
        /// Allows registration of an action that's executed after all other tasks have been run.
        /// If a setup action or a task fails with or without recovery, the specified teardown action will still be executed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        public void Teardown(Action action)
        {
            _teardownAction = action;
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
                throw new CakeException(string.Format(CultureInfo.InvariantCulture, format, target));
            }

            // This isn't pretty, but we need to keep track of exceptions thrown
            // while running a setup action, or a task. We do this since we don't
            // want to throw teardown exceptions if an exception was thrown previously.
            var exceptionWasThrown = false;

            try
            {
                PerformSetup();

                var stopWatch = new Stopwatch();
                var report = new CakeReport();

                foreach (var task in graph.Traverse(target))
                {
                    var taskNode = _tasks.FirstOrDefault(x => x.Name.Equals(task, StringComparison.OrdinalIgnoreCase));
                    Debug.Assert(taskNode != null, "Node should not be null.");

                    var isTarget = taskNode.Name.Equals(target, StringComparison.OrdinalIgnoreCase);

                    if (ShouldTaskExecute(taskNode, isTarget))
                    {
                        _log.Verbose("Executing task: {0}", taskNode.Name);

                        ExecuteTask(stopWatch, taskNode, report);

                        _log.Verbose("Finished executing task: {0}", taskNode.Name);
                    }
                    else
                    {
                        _log.Verbose("Skipping task: {0}", taskNode.Name);
                    }
                }

                return report;
            }
            catch
            {
                exceptionWasThrown = true;
                throw;
            }
            finally
            {
                PerformTeardown(exceptionWasThrown);
            }
        }

        private void PerformSetup()
        {
            if (_setupAction != null)
            {
                _log.Verbose("Executing custom setup action...");
                _setupAction();
            }
        }

        private static bool ShouldTaskExecute(CakeTask task, bool isTarget)
        {
            foreach (var criteria in task.Criterias)
            {
                if (!criteria())
                {
                    if (isTarget)
                    {
                        // It's not OK to skip the target task.
                        // See issue #106 (https://github.com/cake-build/cake/issues/106)
                        const string format = "Could not reach target '{0}' since it was skipped due to a criteria.";
                        var message = string.Format(CultureInfo.InvariantCulture, format, task.Name);
                        throw new CakeException(message);
                    }
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
            catch (Exception taskException)
            {
                _log.Error("An error occured when executing task.", task.Name);

                // Got an error handler?
                if (task.ErrorHandler != null)
                {
                    try
                    {
                        // Let the error handler handle the exception.
                        task.ErrorHandler(taskException);
                    }
                    catch (Exception errorHandlerException)
                    {
                        if (errorHandlerException != taskException)
                        {
                            // Log the original error.
                            _log.Error("Error: {0}", taskException.Message);
                        }
                        // Rethrow the exception and let it propagate.
                        throw;
                    }
                }
                else
                {
                    // No error handler defined for this task.
                    // Rethrow the exception and let it propagate.
                    throw;
                }
            }
            finally
            {
                if (task.FinallyHandler != null)
                {
                    task.FinallyHandler();
                }
            }

            // Add the task results to the report.
            report.Add(task.Name, stopWatch.Elapsed);
        }

        private void PerformTeardown(bool exceptionWasThrown)
        {
            if (_teardownAction != null)
            {
                try
                {
                    _log.Verbose("Executing custom teardown action...");
                    _teardownAction();
                }
                catch (Exception ex)
                {
                    _log.Error("An error occured in the custom teardown action.");
                    if (!exceptionWasThrown)
                    {
                        // If no other exception was thrown, we throw this one.
                        throw;
                    }
                    _log.Error("Teardown error: {0}", ex.ToString());
                }
            }
        }
    }
}
