using System;
using System.Collections.Generic;
using System.Linq;

namespace Cake.Core
{
    /// <inheritdoc/>
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

        /// <inheritdoc/>
        public bool HasArgument(string name)
        {
            return _arguments.ContainsKey(name);
        }

        /// <inheritdoc/>
        public ICollection<string> GetArguments(string name)
        {
            _arguments.TryGetValue(name, out var arguments);
            return arguments ?? (ICollection<string>)Array.Empty<string>();
        }

        /// <inheritdoc/>
        public IDictionary<string, ICollection<string>> GetArguments()
        {
            var arguments = _arguments
                .ToDictionary(x => x.Key, x => (ICollection<string>)x.Value.ToList());

            return arguments;
        }
    }
}
