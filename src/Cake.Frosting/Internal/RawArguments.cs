// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core;

namespace Cake.Frosting.Internal
{
    internal sealed class RawArguments : ICakeArguments
    {
        private readonly Dictionary<string, string> _arguments;

        /// <summary>
        /// Gets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public IReadOnlyDictionary<string, string> Arguments => _arguments;

        /// <summary>
        /// Initializes a new instance of the <see cref="RawArguments" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public RawArguments(CakeHostOptions options)
        {
            _arguments = new Dictionary<string, string>(
                (options ?? new CakeHostOptions()).Arguments ?? new Dictionary<string, string>(),
                StringComparer.OrdinalIgnoreCase);
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
        /// Gets an argument.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <returns>The argument value.</returns>
        public ICollection<string> GetArguments(string name)
        {
            return _arguments.ContainsKey(name)
                ? new[] { _arguments[name] }
                : Array.Empty<string>();
        }
    }
}
