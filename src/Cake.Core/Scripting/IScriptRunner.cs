using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// Represents a script runner responsible for running scripts.
    /// </summary>
    public interface IScriptRunner
    {
        /// <summary>
        /// Runs the script using the specified script host.
        /// </summary>
        /// <param name="host">The script host.</param>
        /// <param name="scriptPath">The script path.</param>
        /// <param name="arguments">The arguments.</param>
        void Run(IScriptHost host, FilePath scriptPath, IDictionary<string, string> arguments);
    }
}