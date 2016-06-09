// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Scripting;

namespace Cake.Scripting
{
    /// <summary>
    /// The script host used for showing task descriptions.
    /// </summary>
    public sealed class DescriptionScriptHost : ScriptHost
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
            if (console == null)
            {
                throw new ArgumentNullException("console");
            }
            _console = console;
            _descriptions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Runs the specified target.
        /// </summary>
        /// <param name="target">The target to run.</param>
        /// <returns>The resulting report.</returns>
        public override CakeReport RunTarget(string target)
        {
            foreach (var task in Tasks)
            {
                _descriptions.Add(task.Name, task.Description);
            }

            _console.WriteLine();
            _console.WriteLine("{0,-30}{1}", "Task", "Description");
            _console.WriteLine(string.Concat(Enumerable.Range(0, 79).Select(s => "=")));
            foreach (var key in _descriptions.Keys)
            {
                _console.WriteLine("{0,-30}{1}", key, _descriptions[key]);
            }

            return null;
        }
    }
}
