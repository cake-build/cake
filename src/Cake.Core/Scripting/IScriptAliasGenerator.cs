using System.Collections.Generic;
using System.Reflection;
using Cake.Core.Scripting.Processing;

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
        /// <param name="context">The context.</param>
        /// <param name="assemblies">The assemblies to find script aliases in.</param>
        void GenerateScriptAliases(ScriptProcessorContext context, IEnumerable<Assembly> assemblies);
    }
}
