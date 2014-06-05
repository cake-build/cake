using System;
using System.Collections.Generic;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake
{
    public sealed class CakeOptions
    {
        private readonly Dictionary<string, string> _arguments; 

        public Verbosity Verbosity { get; set; }
        public FilePath Script { get; set; }

        public IDictionary<string, string> Arguments
        {
            get { return _arguments; }
        }

        public CakeOptions()
        {
            _arguments = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            // Default to normal verbosity.
            Verbosity = Verbosity.Normal;
        }
    }
}
