using System.Collections.Generic;
using Cake.Core.Graph;
using Cake.Core.IO;

namespace Cake.Core
{
    public sealed class CakeEngine : ICakeEngine
    {
        private readonly IFileSystem _fileSystem;
        private readonly List<CakeTask> _tasks;

        public IReadOnlyList<CakeTask> Tasks
        {
            get { return _tasks; }
        }

        public CakeEngine()
            : this(null)
        {
        }

        public CakeEngine(IFileSystem fileSystem)
        {            
            _fileSystem = fileSystem ?? new FileSystem();
            _tasks = new List<CakeTask>();
        }

        public CakeTask Task(string name)
        {
            var task = new CakeTask(name);
            _tasks.Add(task);
            return task;
        }

        public void Run(string target)
        {
            var graph = CakeGraphBuilder.Build(_tasks);

            var context = new CakeContext(_fileSystem);
            foreach (var task in graph.Traverse(target))
            {
                var shouldExecute = true;
                foreach (var criteria in task.Criterias)
                {
                    if (!criteria(context))
                    {
                        shouldExecute = false;
                        break;
                    }
                }
                if (shouldExecute)
                {
                    foreach (var action in task.Actions)
                    {
                        action(context);
                    }
                }
            }
        }
    }
}
