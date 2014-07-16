using System.Collections.Generic;
using System.Reflection;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// Represents a script alias generator.
    /// </summary>
    public interface IScriptAliasGenerator
    {
        /// <summary>
        /// Generates script aliases and adds them to the specified session.
        /// </summary>
        /// <param name="session">The session to add script aliases to.</param>
        /// <param name="assemblies">The assemblies to find script aliases in.</param>
        void Generate(IScriptSession session, IEnumerable<Assembly> assemblies);
    }
}
