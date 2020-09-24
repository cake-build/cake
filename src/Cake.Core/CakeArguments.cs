using System.Collections.Generic;
using System.Linq;

namespace Cake.Core
{
    /// <summary>
    /// Represents arguments passed to the executing script.
    /// </summary>
    public sealed class CakeArguments : ICakeArguments
    {
        private readonly ILookup<string, string> _arguments;

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeArguments"/> class.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        public CakeArguments(ILookup<string, string> arguments)
        {
            _arguments = arguments;
        }

        /// <summary>
        /// Determines whether or not the specified argument exist.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <returns>
        ///   <c>true</c> if the argument exist; otherwise <c>false</c>.
        /// </returns>
        public bool HasArgument(string name)
        {
            return _arguments.Contains(name);
        }

        /// <summary>
        /// Gets all values for an argument.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <returns>The argument values.</returns>
        public ICollection<string> GetArguments(string name)
        {
            return _arguments.Contains(name)
                ? _arguments[name].ToArray()
                : null;
        }
    }
}
