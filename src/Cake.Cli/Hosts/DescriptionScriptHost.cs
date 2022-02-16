// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.Scripting;

namespace Cake.Cli
{
    /// <summary>
    /// The script host used for showing task descriptions.
    /// </summary>
    public class DescriptionScriptHost : ScriptHost
    {
        private readonly IConsole _console;
        private readonly Dictionary<string, string> _descriptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptionScriptHost"/> class.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <param name="context">The context.</param>
        /// <param name="console">The console.</param>
        public DescriptionScriptHost(ICakeEngine engine, ICakeContext context, IConsole console)
            : base(engine, context)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _descriptions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        /// <inheritdoc/>
        public override Task<CakeReport> RunTargetAsync(string target)
        {
            var maxTaskNameLength = 29;

            foreach (var task in Tasks)
            {
                if (task.Name.Length > maxTaskNameLength)
                {
                    maxTaskNameLength = task.Name.Length;
                }

                _descriptions.Add(task.Name, task.Description);
            }

            maxTaskNameLength++;
            string lineFormat = "{0,-" + maxTaskNameLength + "}{1}";

            _console.WriteLine();
            _console.WriteLine(lineFormat, "Task", "Description");
            _console.WriteLine(new String('=', maxTaskNameLength + 50));
            foreach (var key in _descriptions.Keys)
            {
                _console.WriteLine(lineFormat, key, _descriptions[key]);
            }

            return System.Threading.Tasks.Task.FromResult<CakeReport>(null);
        }
    }
}