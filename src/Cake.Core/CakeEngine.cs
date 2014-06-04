using System;
using System.Collections.Generic;
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

        public CakeEngine(IFileSystem fileSystem, ICakeEnvironment environment, ICakeLog log, IGlobber globber)
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
            _fileSystem = fileSystem;
            _environment = environment;
            _log = log;
            _globber = globber ?? new Globber(_fileSystem, _environment);
            _tasks = new List<CakeTask>();
        }

        public CakeTask Task(string name)
        {
            if (_tasks.Any(x => x.Name == name))
            {
                const string format = "Another task with the name '{0}' has already been added.";
                throw new CakeException(string.Format(format, name));
            }
            var task = new CakeTask(name);
            _tasks.Add(task);
            return task;
        }

        public void Run(string target)
        {
            var graph = CakeGraphBuilder.Build(_tasks);

            // Make sure target exist.
            if (graph.Find(target) == null)
            {
                const string format = "The target '{0}' was not found.";
                throw new CakeException(string.Format(format, target));
            }

            foreach (var task in graph.Traverse(target))
            {
                if (ShouldTaskExecute(task))
                {
                    _log.Verbose("Executing task: {0}...", task.Name);
                    foreach (var action in task.Actions)
                    {
                        action(this);
                    }
                    _log.Verbose("Finished executing task: {0}", task.Name);
                }
            }
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
    }
}
