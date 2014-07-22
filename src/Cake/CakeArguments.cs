using System;
using System.Collections.Generic;
using Cake.Core;

namespace Cake
{
    internal sealed class CakeArguments : ICakeArguments
    {
        private readonly Dictionary<string, string> _arguments;

        public IReadOnlyDictionary<string, string> Arguments
        {
            get { return _arguments; }
        }

        public CakeArguments()
        {
            _arguments = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public void SetArguments(IDictionary<string, string> arguments)
        {
            if (_arguments.Count == 0)
            {
                foreach (var argument in arguments)
                {
                    _arguments.Add(argument.Key, argument.Value);
                }
            }
        }

        public bool HasArgument(string name)
        {
            return _arguments.ContainsKey(name);
        }

        public string GetArgument(string name)
        {
            return _arguments.ContainsKey(name) 
                ? _arguments[name] : null;
        }
    }
}
