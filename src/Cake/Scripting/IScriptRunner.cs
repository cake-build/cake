using System.Collections.Generic;
using System.Reflection;

namespace Cake.Scripting
{
    public interface IScriptRunner
    {
        void Run(ScriptHost host, IEnumerable<Assembly> references, IEnumerable<string> namespaces, string code);
    }
}
