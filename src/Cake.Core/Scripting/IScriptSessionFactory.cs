using System.Collections.Generic;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// Represents a script session factory.
    /// </summary>
    public interface IScriptSessionFactory
    {
        /// <summary>
        /// Creates a new script session.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="arguments">The script arguments.</param>
        /// <returns>A new script session.</returns>
        IScriptSession CreateSession(IScriptHost host, IDictionary<string, string> arguments);
    }
}
