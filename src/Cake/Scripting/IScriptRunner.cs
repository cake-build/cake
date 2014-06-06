using System.Collections.Generic;
using System.Reflection;
using Cake.Scripting.Host;

namespace Cake.Scripting
{
    public interface IScriptRunner
    {
        void Run(ScriptHost host, IEnumerable<Assembly> references, IEnumerable<string> namespaces, string code);
    }
}
