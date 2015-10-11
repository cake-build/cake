using System.Collections.Generic;

namespace Cake.Core
{
    /// <summary>
    /// Represents arguments passed to script.
    /// </summary>
    public interface ICakeArguments
    {
        /// <summary>
        /// Initializes the argument list.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        void SetArguments(IDictionary<string, string> arguments);

        /// <summary>
        /// Determines whether or not the specified argument exist.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <returns>
        ///   <c>true</c> if the argument exist; otherwise <c>false</c>.
        /// </returns>
        bool HasArgument(string name);

        /// <summary>
        /// Gets an argument.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <returns>The argument value.</returns>
        string GetArgument(string name);
    }
}