// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.Graph;
using Cake.Core.Scripting;

namespace Cake.Cli
{
    /// <summary>
    /// The script host used for showing task descriptions.
    /// </summary>
    public sealed class TreeScriptHost : ScriptHost
    {
        private const int _maxDepth = 0;
        private const string _cross = "├─";
        private const string _corner = "└─";
        private const string _vertical = "│ ";
        private readonly IConsole _console;

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeScriptHost"/> class.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <param name="context">The context.</param>
        /// <param name="console">The console.</param>
        public TreeScriptHost(ICakeEngine engine, ICakeContext context, IConsole console)
            : base(engine, context)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
        }

        /// <inheritdoc/>
        public override Task<CakeReport> RunTargetAsync(string target)
        {
            var topLevelTasks = GetTopLevelTasks();
            _console.WriteLine();

            foreach (ICakeTaskInfo task in topLevelTasks)
            {
                PrintTask(task, string.Empty, false, 0);
                _console.WriteLine();
            }

            return System.Threading.Tasks.Task.FromResult<CakeReport>(null);
        }

        private List<ICakeTaskInfo> GetTopLevelTasks()
        {
            // Display "Default" first, then alphabetical
            var graph = CakeGraphBuilder.Build(Tasks);
            return Tasks.Where(task => !graph.Edges.Any(
                edge => edge.Start.Equals(task.Name, StringComparison.OrdinalIgnoreCase)))
                .OrderByDescending(task => task.Name.Equals("Default", StringComparison.OrdinalIgnoreCase))
                .ThenBy(task => task.Name, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private void PrintTask(ICakeTaskInfo task, string indent, bool isLast, int depth)
        {
            // Builds ASCII graph
            _console.Write(indent);
            if (isLast)
            {
                _console.Write(_corner);
                indent += "   ";
            }
            else if (depth > 0)
            {
                _console.Write(_cross);
                indent += _vertical;
            }

            PrintName(task, depth);

            if ((_maxDepth > 0) && (depth >= _maxDepth))
            {
                return;
            }

            for (var i = 0; i < task.Dependencies.Count; i++)
            {
                // First() is safe as CakeGraphBuilder has already validated graph is valid
                var childTask = Tasks
                    .Where(x => x.Name.Equals(task.Dependencies[i].Name, StringComparison.OrdinalIgnoreCase))
                    .First();

                PrintTask(childTask, indent, i == (task.Dependencies.Count - 1), depth + 1);
            }
        }

        private void PrintName(ICakeTaskInfo task, int depth)
        {
            var originalColor = _console.ForegroundColor;

            if (depth == 0)
            {
                _console.ForegroundColor = ConsoleColor.Cyan;
            }
            else if (task is CakeTask cakeTask &&
                (cakeTask.Actions.Any() || cakeTask.DelayedActions.Any()))
            {
                _console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                _console.ForegroundColor = ConsoleColor.Gray;
            }

            _console.WriteLine(task.Name);
            _console.ForegroundColor = originalColor;
        }
    }
}