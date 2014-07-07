using System.Collections.Generic;
using System.Reflection;

namespace Cake.Core.Scripting
{
    public interface IScriptAliasGenerator
    {
        void Generate(IScriptSession session, IEnumerable<Assembly> assemblies);
    }
}
