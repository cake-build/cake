using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Cake.Core.Diagnostics;
using Cake.Core.Graph;

namespace Cake.Core
{
    /// <summary>
    /// The Cake execution engine.
    /// </summary>
    public sealed class CakeEngine : ICakeEngine
    {
        private readonly ICakeLog _log;
        private readonly List<CakeTask> _tasks;
        private Action _setupAction;
        private Action _teardownAction;
        private string _target;

        /// <summary>
        /// Gets all registered tasks.
        /// </summary>
        /// <value>The registered tasks.</value>
        public IReadOnlyList<CakeTask> Tasks
        {
            get { return _tasks; }
        }

        /// <summary>
        /// Gets or sets the name of the target to run.
        /// When null or empty, defaults to "Default".
        /// </summary>
        public string Target
        {
            get
            {
                return string.IsNullOrEmpty(_target) ? "Default" : _target; 
            }

            set
            {
                _target = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeEngine"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        public CakeEngine(ICakeLog log)
        {
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }
            _log = log;
            _tasks = new List<CakeTask>();
        }

        /// <summary>
        /// Creates and registers a new <see cref="ActionTask"/>.
        /// </summary>
        /// <param name="name">The name of the task.</param>
        /// <returns>
        /// A <see cref="CakeTaskBuilder{T}"/> used to configure the task.
        /// </returns>
        public CakeTaskBuilder<ActionTask> RegisterTask(string name)
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
        public void RegisterSetupAction(Action action)
        {
            _setupAction = action;
        }

        /// <summary>
        /// Allows registration of an action that's executed after all other tasks have been run.
        /// If a setup action or a task fails with or without recovery, the specified teardown action will still be executed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        public void RegisterTeardownAction(Action action)
        {
            _teardownAction = action;
        }

        /// <summary>
        /// Runs the specified target.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="strategy">The execution strategy.</param>
        /// <param name="target">The target to run. When let to null, <see cref="Target"/> property is used.</param>
        /// <returns>The resulting report.</returns>
        public CakeReport RunTarget(ICakeContext context, IExecutionStrategy strategy, string target = null)
        {
            if (strategy == null)
            {
                throw new ArgumentNullException("strategy");
            }
            if (target == null)
            {
                target = Target;
            }

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
                PerformSetup(strategy);

                var stopWatch = new Stopwatch();
                var report = new CakeReport();

                foreach (var taskNode in graph.Traverse(target))
                {
                    // Get the task.
                    var task = _tasks.FirstOrDefault(x => x.Name.Equals(taskNode, StringComparison.OrdinalIgnoreCase));
                    Debug.Assert(task != null, "Node should not be null.");

                    // Is this the current target?
                    var isTarget = task.Name.Equals(target, StringComparison.OrdinalIgnoreCase);

                    // Should we execute the task?
                    if (ShouldTaskExecute(task, isTarget))
                    {
                        ExecuteTask(context, strategy, stopWatch, task, report);
                    }
                    else
                    {
                        SkipTask(strategy, task);
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
                PerformTeardown(strategy, exceptionWasThrown);
            }
        }

        private void PerformSetup(IExecutionStrategy strategy)
        {
            if (_setupAction != null)
            {
                strategy.PerformSetup(_setupAction);
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

        private void ExecuteTask(ICakeContext context, IExecutionStrategy strategy, Stopwatch stopWatch, CakeTask task, CakeReport report)
        {
            // Reset the stop watch.
            stopWatch.Reset();
            stopWatch.Start();

            try
            {
                // Execute the task.
                strategy.Execute(task, context);
            }
            catch (Exception exception)
            {
                _log.Error("An error occured when executing task.", task.Name);

                // Got an error reporter?
                if (task.ErrorReporter != null)
                {
                    ReportErrors(strategy, task.ErrorReporter, exception);
                }

                // Got an error handler?
                if (task.ErrorHandler != null)
                {
                    HandleErrors(strategy, task.ErrorHandler, exception);
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
                    strategy.InvokeFinally(task.FinallyHandler);
                }
            }

            // Add the task results to the report.
            report.Add(task.Name, stopWatch.Elapsed);
        }

        private static void SkipTask(IExecutionStrategy strategy, CakeTask task)
        {
            strategy.Skip(task);
        }

        private static void ReportErrors(IExecutionStrategy strategy, Action<Exception> errorReporter, Exception taskException)
        {
            try
            {
                strategy.ReportErrors(errorReporter, taskException);
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
                // Ignore errors from the error reporter.
            }
        }

        private void HandleErrors(IExecutionStrategy strategy, Action<Exception> errorHandler, Exception exception)
        {
            try
            {
                strategy.HandleErrors(errorHandler, exception);
            }
            catch (Exception errorHandlerException)
            {
                if (errorHandlerException != exception)
                {
                    _log.Error("Error: {0}", exception.Message);
                }
                throw;
            }
        }

        private void PerformTeardown(IExecutionStrategy strategy, bool exceptionWasThrown)
        {
            if (_teardownAction != null)
            {
                try
                {
                    strategy.PerformTeardown(_teardownAction);
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
