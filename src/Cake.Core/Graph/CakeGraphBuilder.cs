// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Globalization;

namespace Cake.Core.Graph
{
    /// <summary>
    /// Responsible for building the Cake task graph.
    /// </summary>
    public static class CakeGraphBuilder
    {
        /// <summary>
        /// Builds a <see cref="CakeGraph"/> from the specified tasks.
        /// </summary>
        /// <param name="tasks">The tasks to build the graph from.</param>
        /// <returns>An assembled graph.</returns>
        public static CakeGraph Build(IReadOnlyCollection<ICakeTaskInfo> tasks)
        {
            var graph = new CakeGraph();
            foreach (var task in tasks)
            {
                graph.Add(task.Name);
            }
            foreach (var task in tasks)
            {
                foreach (var dependency in task.Dependencies)
                {
                    if (!graph.Exist(dependency.Name))
                    {
                        if (dependency.Required)
                        {
                            const string format = "Task '{0}' is dependent on task '{1}' which does not exist.";
                            var message = string.Format(CultureInfo.InvariantCulture, format, task.Name, dependency.Name);
                            throw new CakeException(message);
                        }
                    }
                    else
                    {
                        graph.Connect(dependency.Name, task.Name);
                    }
                }

                foreach (var dependency in task.Dependees)
                {
                    if (!graph.Exist(dependency.Name))
                    {
                        if (dependency.Required)
                        {
                            const string format = "Task '{0}' has specified that it's a dependency for task '{1}' which does not exist.";
                            var message = string.Format(CultureInfo.InvariantCulture, format, task.Name, dependency.Name);
                            throw new CakeException(message);
                        }
                    }
                    else
                    {
                        graph.Connect(task.Name, dependency.Name);
                    }
                }
            }
            return graph;
        }
    }
}