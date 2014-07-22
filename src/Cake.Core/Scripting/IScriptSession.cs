using System.Reflection;
using Cake.Core.IO;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// Represents a script session.
    /// </summary>
    public interface IScriptSession
    {
        /// <summary>
        /// Adds a reference path to the session.
        /// </summary>
        /// <param name="path">The reference path.</param>
        void AddReferencePath(FilePath path);

        /// <summary>
        /// Adds an assembly reference to the session.
        /// </summary>
        /// <param name="assembly">The assembly reference.</param>
        void AddReference(Assembly assembly);

        /// <summary>
        /// Imports a namespace to the session.
        /// </summary>
        /// <param name="namespace">The namespace to import.</param>
        void ImportNamespace(string @namespace);

        /// <summary>
        /// Executes the specified script code.
        /// </summary>
        /// <param name="code">The script code to execute.</param>
        void Execute(string code);
    }
}
