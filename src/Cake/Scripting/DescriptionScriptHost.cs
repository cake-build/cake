using System;
using System.Collections.Generic;
using Cake.Core;

namespace Cake.Scripting
{
    public sealed class DescriptionScriptHost : ScriptHost
    {
        public Dictionary<string, string> Tasks { get; set; } 

        public DescriptionScriptHost(ICakeEngine engine) : base(engine)
        {
            Tasks = new Dictionary<string, string>();
        }

        public new CakeReport RunTarget(string target)
        {
            foreach (var t in _engine.Tasks)
            {
                Tasks.Add(t.Name, t.Description);
            }

            return new CakeReport();
        }
    }
}