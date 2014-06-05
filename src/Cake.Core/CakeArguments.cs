using System;
using System.Collections.Generic;

namespace Cake.Core
{
    public sealed class CakeArguments : ICakeArguments
    {
        private readonly IDictionary<string, string> _arguments;

        public CakeArguments(IDictionary<string, string> arguments = null)
        {
            _arguments = new Dictionary<string, string>(
                arguments ?? new Dictionary<string, string>(), 
                StringComparer.OrdinalIgnoreCase);
        }

        public bool HasArgument(string key)
        {
            return _arguments.ContainsKey(key);
        }

        public string GetArgument(string key)
        {
            return _arguments.ContainsKey(key) 
                ? _arguments[key] : null;
        }
    }
}
