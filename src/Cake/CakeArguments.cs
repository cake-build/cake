// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;

namespace Cake
{
    internal sealed class CakeArguments : ICakeArguments
    {
        private readonly Dictionary<string, string> _arguments;
        private readonly ISet<string> _definedArgumentNames;

        /// <summary>
        /// Gets the arguments provided via the command line and their specified values.
        /// </summary>
        public IReadOnlyDictionary<string, string> AsDictionary
        {
            get { return _arguments; }
        }

        /// <summary>
        /// Gets the argument names defined within the executing the Cake script.
        /// </summary>
        public IEnumerable<string> DefinedArgumentNames
        {
            get { return _definedArgumentNames; }
        }

        /// <summary>
        /// Gets the argument names provided via the command line that have not been defined within the executing Cake script.
        /// </summary>
        public IEnumerable<string> UnrecognizedArgumentNames
        {
            get { return _arguments.Keys.Except(_definedArgumentNames, StringComparer.OrdinalIgnoreCase); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeArguments" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public CakeArguments(CakeOptions options)
        {
            _arguments = new Dictionary<string, string>(
                (options ?? new CakeOptions()).Arguments ?? new Dictionary<string, string>(),
                StringComparer.OrdinalIgnoreCase);

            _definedArgumentNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines whether or not the specified argument exist.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <returns>
        ///   <c>true</c> if the argument exists; otherwise <c>false</c>.
        /// </returns>
        public bool HasArgument(string name)
        {
            name = name.Trim();

            return _arguments.ContainsKey(name);
        }

        /// <summary>
        /// Gets an argument.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <returns>The argument value if the argument exists, otherwise <c>null</c>.</returns>
        public string GetArgument(string name)
        {
            name = name.Trim();

            _definedArgumentNames.Add(name);

            return _arguments.ContainsKey(name)
                ? _arguments[name] : null;
        }
    }
}
