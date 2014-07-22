using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Scripting;

namespace Cake.Scripting.Hosts
{
    public sealed class DescriptionScriptHost : ScriptHost
    {
        private readonly IConsole _console;
        private readonly Dictionary<string, string> _descriptions;

        public DescriptionScriptHost(ICakeEngine engine, IConsole console)
            : base(engine)
        {
            _console = console;
            _descriptions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

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
