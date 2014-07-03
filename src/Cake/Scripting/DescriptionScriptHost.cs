using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Scripting;

namespace Cake.Scripting
{
    public sealed class DescriptionScriptHost : ScriptHost
    {
        private readonly Dictionary<string, string> _descriptions; 

        public DescriptionScriptHost(ICakeEngine engine) 
            : base(engine)
        {
            _descriptions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public override CakeReport RunTarget(string target)
        {
            foreach (var task in Tasks)
            {
                _descriptions.Add(task.Name, task.Description);
            }

            Console.WriteLine("{0,-30}{1}", "Task", "Description");
            Console.WriteLine(String.Concat(Enumerable.Range(0, 79).Select(s => "=")));
            foreach (var key in _descriptions.Keys.OrderByDescending(s => s))
            {
                Console.WriteLine("{0,-30}{1}", key, _descriptions[key]);
            }

            return new CakeReport();
        }
    }
}