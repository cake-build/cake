using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Cake.Core.Diagnostics;
using Cake.Core.Graph;
using Cake.Core.IO;

namespace Cake.Core
{
    public sealed class CakeEngine : ICakeEngine
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly IGlobber _globber;
        private readonly ICakeLog _log;
        private readonly ICakeArguments _arguments;
        private readonly IProcessRunner _processRunner;
        private readonly List<CakeTask> _tasks;

        public IFileSystem FileSystem
        {
            get { return _fileSystem; }
        }

        public ICakeEnvironment Environment
        {
            get { return _environment; }
        }

        public IReadOnlyList<CakeTask> Tasks
        {
            get { return _tasks; }
        }

        public IGlobber Globber
        {
            get { return _globber; }
        }

        public ICakeLog Log
        {
            get { return _log; }
        }

        public ICakeArguments Arguments
        {
            get { return _arguments; }
        }

        public IProcessRunner ProcessRunner
        {
            get { return _processRunner; }
        }

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

        public CakeTaskBuilder<T> Build<T>()
            where T : CakeTask, new()
        {
            var task = new T();
            _tasks.Add(task);
            var builder = new CakeTaskBuilder<T>(task);
            return builder;
        }

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
                    _log.Verbose("Executing task: {0}...", taskNode.Name);

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
