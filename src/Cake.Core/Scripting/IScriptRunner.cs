using System.Collections.Generic;
using System.Reflection;

namespace Cake.Core.Scripting
{
    public interface IScriptRunner
    {
        void Run(IScriptHost host, IEnumerable<Assembly> references, IEnumerable<string> namespaces, string code);
    }
}
