using System;
using System.Collections.Generic;

namespace Cake.Core.Graph
{
    internal static class CakeGraphBuilder
    {
        public static CakeGraph Build(IEnumerable<CakeTask> tasks)
        {
            return Build(new List<CakeTask>(tasks));
        }

        private static CakeGraph Build(List<CakeTask> tasks)
        {
            var graph = new CakeGraph();
            foreach (var task in tasks)
            {
                graph.Add(task);
            }
            foreach (var task in tasks)
            {
                foreach (var dependency in task.Dependencies)
                {
                    var taskDependency = graph.Find(dependency);
                    if (taskDependency == null)
                    {
                        const string format = "Task '{0}' is dependent on task '{1}' which do not exist.";
                        var message = string.Format(format, task.Name, dependency);
                        throw new InvalidOperationException(message);
                    }

                    graph.Connect(taskDependency, task);
                }
            }
            return graph;
        }
    }
}
