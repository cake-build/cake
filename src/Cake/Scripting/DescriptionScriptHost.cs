using System;
using System.Collections.Generic;
using Cake.Core;

namespace Cake.Scripting
{
    public sealed class DescriptionScriptHost : ScriptHost
    {
        public Dictionary<string, string> TasksWithDescription { get; set; } 

        public DescriptionScriptHost(ICakeEngine engine) : base(engine)
        {
            TasksWithDescription = new Dictionary<string, string>();
        }

        public new CakeReport RunTarget(string target)
        {
            foreach (var t in Tasks)
            {
                TasksWithDescription.Add(t.Name, t.Description);
            }

            return new CakeReport();
        }
    }
}