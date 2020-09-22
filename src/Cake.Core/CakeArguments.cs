using System;
using System.Collections.Generic;
using System.Linq;

namespace Cake.Core
{
    /// <summary>
    /// Represents arguments passed to the executing script.
    /// </summary>
    public sealed class CakeArguments : ICakeArguments
    {
        private readonly Dictionary<string, List<string>> _arguments;

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeArguments"/> class.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        public CakeArguments(ILookup<string, string> arguments)
        {
            _arguments = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
            foreach (var group in arguments)
            {
                _arguments[group.Key] = new List<string>();
                foreach (var argument in group)
                {
                    _arguments[group.Key].Add(argument);
                }
            }
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
            return _arguments.ContainsKey(name);
        }

        /// <summary>
        /// Gets all values for an argument.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <returns>The argument values.</returns>
        public ICollection<string> GetArguments(string name)
        {
            _arguments.TryGetValue(name, out var arguments);
            return arguments ?? (ICollection<string>)Array.Empty<string>();
        }
    }
}
